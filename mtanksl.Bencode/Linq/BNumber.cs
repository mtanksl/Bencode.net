using System;

namespace mtanksl.Bencode.Linq
{
    public class BNumber : BElement
    {
        /// <exception cref="ArgumentNullException"></exception>
        /// 
        public BNumber(BNumber value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value) );
            }

            Value = value.Value;
        }

        public BNumber(long value)
        {
            Value = value;
        }

        public long Value { get; }

        public static implicit operator BNumber(long value)
        {
            return new BNumber(value);
        }

        public static explicit operator long?(BNumber value)
        {
            if (value == null)
            {
                return null;
            }

            return value.Value;
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// 
        public static explicit operator long(BNumber value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value) );
            }

            return value.Value;
        }
    }
}