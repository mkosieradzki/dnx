// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Dnx.Runtime
{
    /// <summary>
    /// Exposes information about a library which can be an assembly, project, or a package.
    /// </summary>
    public class Library
    {
        private Dictionary<string, object> _properties = new Dictionary<string, object>();

        public Library(string name)
            : this(name, string.Empty, string.Empty, string.Empty, Enumerable.Empty<string>(), Enumerable.Empty<AssemblyName>())
        {
        }

        public Library(string name, IEnumerable<string> dependencies)
            : this(name, string.Empty, string.Empty, string.Empty, dependencies, Enumerable.Empty<AssemblyName>())
        {
        }

        public Library(string name, string version, string path, string type, IEnumerable<string> dependencies, IEnumerable<AssemblyName> assemblies)
        {
            Name = name;
            Version = version;
            Path = path;
            Type = type;
            Dependencies = dependencies;
            Assemblies = assemblies;
        }

        /// <summary>
        /// Gets the name of the library.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///  Gets the version of the library.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Gets the path to the library. For projects, this is a path to the project.json file.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets the type of library. Common values include Project, Package, and Assembly.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets a list of dependencies for the library. The dependencies are names of other <see cref="Library"/> objects.
        /// </summary>
        public IEnumerable<string> Dependencies { get; }

        /// <summary>
        /// Gets a list of assembly names from the library that can be loaded. Packages can contain multiple assemblies.
        /// </summary>
        public IEnumerable<AssemblyName> Assemblies { get; }

        /// <summary>
        /// Gets or sets a property in the Library's property bag.
        /// </summary>
        /// <param name="name">The name of the property to set</param>
        /// <returns>The value of the property</returns>
        public object this[string name]
        {
            get { return _properties[name]; }
            set { _properties[name] = value; }
        }

        /// <summary>
        /// Retrieves a property from the Library's property bag.
        /// </summary>
        /// <remarks>
        /// Well-known property keys can be found in static members on <see cref="LibraryProperties"/>. These
        /// properties are generally only used for library exporting and loading.
        /// </remarks>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value of the property, or the default value for <see cref="T"/> if the property does not exist.</returns>
        public T GetProperty<T>(string name) where T : class
        {
            object val;
            if(!_properties.TryGetValue(name, out val))
            {
                return default(T);
            }
            return (T)val;
        }
    }
}
