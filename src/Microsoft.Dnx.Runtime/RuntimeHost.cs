using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Dnx.Runtime
{
    public class RuntimeHost
    {
        public RuntimeHost(string rootDirectory, string packagesDirectory, Project project, LibraryManager libraryManager)
        {
            RootDirectory = rootDirectory;
            PackagesDirectory = packagesDirectory;
            Project = project;
            LibraryManager = libraryManager;
        }

        /// <summary>
        /// Gets a <see cref="LibraryManager"/> which manages all libraries referenced by the application.
        /// </summary>
        public LibraryManager LibraryManager { get; }

        /// <summary>
        /// Gets the path from which NuGet packages are loaded (or to which they are installed).
        /// </summary>
        public string PackagesDirectory { get; set; }

        /// <summary>
        /// Gets the root project for the runtime
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// Gets the path to the root directory for the runtime host. This is the path to the
        /// global.json file, if present and is used as the base path when searching for projects.
        /// </summary>
        public string RootDirectory { get; }
    }
}
