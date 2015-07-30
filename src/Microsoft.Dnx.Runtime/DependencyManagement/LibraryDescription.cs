// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

namespace Microsoft.Dnx.Runtime
{
    public class LibraryDescription
    {
        private Dictionary<string, object> _properties = new Dictionary<string, object>();
        
        public LibraryRange LibraryRange { get; set; }
        public LibraryIdentity Identity { get; set; }
        public IEnumerable<LibraryDependency> Dependencies { get; set; }

        public bool Resolved { get; set; } = true;
        public bool Compatible { get; set; } = true;

        public string Path { get; set; }
        public string Type { get; set; }
        public FrameworkName Framework { get; set; }
        public IEnumerable<string> LoadableAssemblies { get; set; }

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

        public Library ToLibrary()
        {
            return new Library(
                Identity.Name,
                Identity.Version?.GetNormalizedVersionString(),
                Path,
                Type,
                Dependencies.Select(d => d.Name),
                LoadableAssemblies.Select(a => new AssemblyName(a)));
        }
    }
}
