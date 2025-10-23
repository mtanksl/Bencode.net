using System;

namespace mtanksl.Bencode.Linq
{
    public class BString : BElement, IComparable<BString>
    {
        public BString(BString value) : this(value.Value)
        {
        }

        public BString(string value)
        {
            if (value == null)
            {
                value = "";
            }

            Value = value;
        }

        public string Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(BString other)
        {
            return Value.CompareTo(other.Value);
        }

        public static implicit operator BString(string value)
        {
            return new BString(value);
        }

        public static explicit operator string(BString value)
        {
            if (value == null)
            {
                return null;
            }

            return value.Value;
        }
    }
}