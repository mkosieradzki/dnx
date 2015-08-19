using System;
using System.Collections.Generic;
using System.Runtime.Versioning;

namespace Microsoft.Dnx.Runtime
{
    public class ProjectGraphProvider : IProjectGraphProvider
    {
        public LibraryManager GetProjectGraph(Project project, FrameworkName targetFramework)
        {
            // TODO: Cache sub-graph walk?

            // Create a child app context for this graph walk
            var runtimeHost = RuntimeHostBuilder.Build(project, targetFramework);

            // Return the results
            return runtimeHost.LibraryManager;
        }
    }
}