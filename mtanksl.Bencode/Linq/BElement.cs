using System;

namespace mtanksl.Bencode.Linq
{
    public abstract class BElement
    {
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="FormatException"></exception>
        /// 
        public virtual BElement this[object key]
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public static implicit operator BElement(string value)
        {
            return new BString(value);
        }

        /// <exception cref="FormatException"></exception>
        /// 
        public static explicit operator string(BElement value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is BString)
            {
                return ( (BString)value).Value;
            }

            throw new FormatException();
        }

        public static implicit operator BElement(long value)
        {
            return new BNumber(value);
        }

        /// <exception cref="FormatException"></exception>
        /// 
        public static explicit operator long?(BElement value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is BNumber)
            {
                return ( (BNumber)value).Value;
            }

            throw new FormatException();
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        /// 
        public static explicit operator long(BElement value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value) );
            }

            if (value is BNumber)
            {
                return ( (BNumber)value).Value;
            }

            throw new FormatException();
        }
    }
}