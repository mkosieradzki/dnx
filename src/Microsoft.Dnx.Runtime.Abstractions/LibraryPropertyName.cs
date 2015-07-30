namespace Microsoft.Dnx.Runtime
{
    public struct LibraryPropertyName<T> where T : class
    {
        public string Name { get; }

        public LibraryPropertyName(string name)
        {
            Name = name;
        }
    }
}