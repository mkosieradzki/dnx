using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace Microsoft.Dnx.Runtime
{
    public struct RuntimeHostBuilder
    {
        public Project Project { get; set; }

        public FrameworkName TargetFramework { get; set; }

        public string RootDirectory { get; set; }

        public string PackagesDirectory { get; set; }

        public bool SkipLockfileValidation { get; set; }

        public FrameworkReferenceResolver FrameworkResolver { get; set; }

        public RuntimeHost Build()
        {
            // Initialize defaults
            var rootDirectory = RootDirectory ?? ProjectResolver.ResolveRootDirectory(Project.ProjectDirectory);
            var packagesDirectory = PackagesDirectory ?? PackageDependencyProvider.ResolveRepositoryPath(rootDirectory);
            var frameworkReferenceResolver = FrameworkResolver ?? new FrameworkReferenceResolver();

            // Create support objects
            var projectResolver = new ProjectResolver(Project.ProjectDirectory, rootDirectory);
            var referenceAssemblyDependencyResolver = new ReferenceAssemblyDependencyResolver(frameworkReferenceResolver);
            var gacDependencyResolver = new GacDependencyResolver();
            var projectDependencyProvider = new ProjectReferenceDependencyProvider(projectResolver);
            var unresolvedDependencyProvider = new UnresolvedDependencyProvider();

            DependencyWalker dependencyWalker = null;
            LockFileLookup lockFileLookup = null;

            var projectLockJsonPath = Path.Combine(Project.ProjectDirectory, LockFileReader.LockFileName);
            var lockFileExists = File.Exists(projectLockJsonPath);
            var validLockFile = false;
            var skipLockFileValidation = SkipLockfileValidation;
            string lockFileValidationMessage = null;

            if (lockFileExists)
            {
                var lockFileReader = new LockFileReader();
                var lockFile = lockFileReader.Read(projectLockJsonPath);
                validLockFile = lockFile.IsValidForProject(Project, out lockFileValidationMessage);

                // When the only invalid part of a lock file is version number,
                // we shouldn't skip lock file validation because we want to leave all dependencies unresolved, so that
                // VS can be aware of this version mismatch error and automatically do restore
                skipLockFileValidation &= lockFile.Version == Constants.LockFileVersion;

                if (validLockFile || skipLockFileValidation)
                {
                    lockFileLookup = new LockFileLookup(lockFile);
                    var packageDependencyProvider = new PackageDependencyProvider(PackagesDirectory, lockFileLookup);

                    dependencyWalker = new DependencyWalker(new IDependencyProvider[] {
                        projectDependencyProvider,
                        packageDependencyProvider,
                        referenceAssemblyDependencyResolver,
                        gacDependencyResolver,
                        unresolvedDependencyProvider
                    });
                }
            }

            if ((!validLockFile && !skipLockFileValidation) || !lockFileExists)
            {
                // We don't add NuGetDependencyProvider to DependencyWalker
                // It will leave all NuGet packages unresolved and give error message asking users to run "dnu restore"
                dependencyWalker = new DependencyWalker(new IDependencyProvider[] {
                    projectDependencyProvider,
                    referenceAssemblyDependencyResolver,
                    gacDependencyResolver,
                    unresolvedDependencyProvider
                });
            }

            dependencyWalker.Walk(Project.Name, Project.Version, TargetFramework);

            var libraryManager = new LibraryManager(Project.ProjectFilePath, TargetFramework, dependencyWalker.Libraries);

            if (!validLockFile)
            {
                libraryManager.AddGlobalDiagnostics(new DiagnosticMessage(
                    $"{lockFileValidationMessage}. Please run \"dnu restore\" to generate a new lock file.",
                    Path.Combine(Project.ProjectDirectory, LockFileReader.LockFileName),
                    DiagnosticMessageSeverity.Error));
            }

            if (!lockFileExists)
            {
                libraryManager.AddGlobalDiagnostics(new DiagnosticMessage(
                    $"The expected lock file doesn't exist. Please run \"dnu restore\" to generate a new lock file.",
                    Path.Combine(Project.ProjectDirectory, LockFileReader.LockFileName),
                    DiagnosticMessageSeverity.Error));
            }

            // Return the constructed runtime host
            return new RuntimeHost(rootDirectory, packagesDirectory, Project, libraryManager);
        }

        public static RuntimeHost Build(string projectDirectory, FrameworkName targetFramework)
        {
            // Load the project out of the specified directory
            Project project;
            if (!Project.TryGetProject(projectDirectory, out project))
            {
                throw new InvalidOperationException($"Unable to resolve project from {projectDirectory}");
            }

            return new RuntimeHostBuilder()
            {
                Project = project,
                TargetFramework = targetFramework
            }.Build();
        }

        public static RuntimeHost Build(Project project, FrameworkName targetFramework)
        {
            return new RuntimeHostBuilder()
            {
                Project = project,
                TargetFramework = targetFramework
            }.Build();
        }

        public static RuntimeHost Build(Project project, FrameworkName targetFramework, FrameworkReferenceResolver frameworkReferenceResolver, bool skipLockFileValidation)
        {
            return new RuntimeHostBuilder()
            {
                Project = project,
                TargetFramework = targetFramework,
                FrameworkResolver = frameworkReferenceResolver,
                SkipLockfileValidation = skipLockFileValidation
            }.Build();
        }
    }
}
