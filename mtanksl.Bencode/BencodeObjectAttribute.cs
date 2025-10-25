using System;

namespace mtanksl.Bencode
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]

    public sealed class BencodeObjectAttribute : Attribute
    {
        public Type ItemConverterType { get; set; }

        public object[] ItemConverterParameters { get; set; }
    }
}