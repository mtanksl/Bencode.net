using System;

namespace mtanksl.Bencode
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]

    public sealed class BencodePropertyAttribute : Attribute
    {
        public BencodePropertyAttribute()
        {            
        }

        public BencodePropertyAttribute(string propertyName)
        {
            this.propertyName = propertyName;
        }

        private string propertyName;

        public string PropertyName
        {
            get 
            { 
                return propertyName; 
            }
        }

        public Type ItemConverterType { get; set; }

        public object[] ItemConverterParameters { get; set; }
    }
}