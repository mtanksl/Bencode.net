using System;
using System.Text;

namespace mtanksl.Bencode.Linq
{
    public class BString : BElement, IComparable<BString>
    {
        /// <exception cref="ArgumentNullException"></exception>
        /// 
        public BString(BString value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value) );
            }

            Value = value.Value;
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// 
        public BString(byte[] value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value) );
            }

            Value = value;
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// 
        public BString(string value)
        {
            if (value == null)
            {
                value = "";
            }

            Value = Encoding.UTF8.GetBytes(value);
        }

        public byte[] Value { get; }

        public int CompareTo(BString other)
        {
            int length = Math.Min(Value.Length, other.Value.Length);

            for (int i = 0; i < length; i++)
            {
                if (Value[i] < other.Value[i] )
                {
                    return -1;
                }

                if (Value[i] > other.Value[i] )
                {
                    return 1;
                }
            }

            if (Value.Length < other.Value.Length)
            {
                return -1;
            }

            if (Value.Length > other.Value.Length)
            {
                return 1;
            }

            return 0;
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

            return Encoding.UTF8.GetString(value.Value);
        }

        public static explicit operator byte[](BString value)
        {
            if (value == null)
            {
                return null;
            }

            return value.Value;
        }
    }
}