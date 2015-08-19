using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Microsoft.Dnx.Runtime
{
    public class LibraryRange : IEquatable<LibraryRange>
    {
        public string Name { get; }

        public SemanticVersionRange VersionRange { get; set; }

        public IEnumerable<string> AllowedTypes { get; }

        // Information for the editor
        public string FileName { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public LibraryRange(string name): this(name, allowedTypes: Enumerable.Empty<string>()) { }

        public LibraryRange(string name, IEnumerable<string> allowedTypes)
        {
            Name = name;

            AllowedTypes = new List<string>(allowedTypes);
        }

        public override string ToString()
        {
            return 
                (!AllowedTypes.Any() ? "*" : string.Join("||", AllowedTypes)) + "/" +
                Name + " " + 
                VersionRange?.ToString();
        }

        public bool Equals(LibraryRange other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) &&
                Equals(VersionRange, other.VersionRange) &&
                Equals(AllowedTypes, other.AllowedTypes);
        }

        public bool AllowsType(string type)
        {
            // The range must be either unconstrained or have the specified type as an allowed type
            return !AllowedTypes.Any() || AllowedTypes.Contains(type, StringComparer.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LibraryRange)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^
                    (VersionRange != null ? VersionRange.GetHashCode() : 0) ^
                    (AllowedTypes.GetHashCode());
            }
        }

        public static bool operator ==(LibraryRange left, LibraryRange right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LibraryRange left, LibraryRange right)
        {
            return !Equals(left, right);
        }
    }
}
