using mtanksl.Bencode.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mtanksl.Bencode
{
    public class BencodeReader : IDisposable
    {
        private Stream stream;

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// 
        public BencodeReader(Stream stream)
        {
			if (stream == null)
			{
				throw new ArgumentNullException(nameof(stream) );
			}

            if ( !stream.CanRead)
            {
				throw new NotSupportedException();
            }

			this.stream = stream;
        }

        private byte? next;

        /// <exception cref="BencodeException"></exception>
        /// 
        public byte PeekByte()
        {
            if (next == null)
            {
                var value = stream.ReadByte();

                if (value == -1)
                {
                    throw new BencodeException("End of stream.");
                }

                next = (byte)value;
            }

            return next.Value;
        }

        /// <exception cref="BencodeException"></exception>
        /// 
        public byte ReadByte()
        {
            if (next != null)
            {
                var buffer = next.Value; 
                
                    next = null;

                return buffer;
            }
            else
            {
                var value = stream.ReadByte();

                if (value == -1)
                {
                    throw new BencodeException("End of stream.");
                }

                return (byte)value;
            }
        }

        /// <exception cref="BencodeException"></exception>
        /// 
        public byte[] ReadBytes(int length)
        {
            if (length == 0)
            {
                return Array.Empty<byte>();
            }

            if (next != null)
            {
                var buffer = new byte[length]; 
                
                    buffer[0] = next.Value; 
                
                    next = null;

                int offset = 1;

                int remaining = buffer.Length - 1;

                while (remaining > 0)
                {
                    int read = stream.Read(buffer, offset, remaining);

                    if (read == 0)
                    {
                        throw new BencodeException("End of stream.");
                    }

                    offset += read;

                    remaining -= read;
                }

                return buffer;
            }
            else
            {
                var buffer = new byte[length];

                int offset = 0;

                int remaining = buffer.Length;

                while (remaining > 0)
                {
                    int read = stream.Read(buffer, offset, remaining);

                    if (read == 0)
                    {
                        throw new BencodeException("End of stream.");
                    }

                    offset += read;

                    remaining -= read;
                }

                return buffer;
            }
        }

        /// <exception cref="BencodeException"></exception>
        /// 
        public string ReadUntil(char character)
        {
            var stringBuilder = new StringBuilder();

            while (true)
            {
                var value = (char)ReadByte();

                if (value == character)
                {
                    break;
                }

                stringBuilder.Append(value);
            }

            return stringBuilder.ToString();
        }

        internal Func<Type, object> ReadObjectHandler { get; set; }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public object ReadObject()
        {
            return ReadObject(typeof(BElement) );
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public T ReadObject<T>()
        {
            return (T)ReadObject(typeof(T) );
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public object ReadObject(Type type)
		{
            if (type == null)
            {
				throw new ArgumentNullException(nameof(type) );
            }
                                 
            if (type == typeof(string) )
            {
                return ReadString();
            }
            else if (type == typeof(byte[] ) )
            {
                return ReadByteString();
            }
            else if (type == typeof(BString) )
            {
                return new BString(ReadByteString() );
            }
            else if (type == typeof(sbyte) || type == typeof(byte) || type == typeof(short) || type == typeof(ushort) || type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) )
            {
                var value = ReadInteger();

                if (value != null)
                {
                    return Convert.ChangeType(value.Value, type);
                }

                return 0;
            }
            else if (type == typeof(sbyte?) || type == typeof(byte?) || type == typeof(short?) || type == typeof(ushort?) || type == typeof(int?) || type == typeof(uint?) || type == typeof(long?) || type == typeof(ulong?) )
            {
                var value = ReadInteger();

                if (value != null)
                {
                    return Convert.ChangeType(value.Value, Nullable.GetUnderlyingType(type) );
                }

                return null;
            }
            else if (type == typeof(BNumber) )
            {
                return new BNumber(ReadInteger() );
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) )
            {
                var genericArguments = type.GetGenericArguments();

                return ReadList(genericArguments[0] );
            }
            else if (type == typeof(BList) )
            {
                return new BList( (IList<BElement>)ReadList(typeof(BElement) ) );
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SortedDictionary<,>) )
            {
                var genericArguments = type.GetGenericArguments();

                return ReadDictionary(genericArguments[0], genericArguments[1] );
            }
            else if (type == typeof(BDictionary) )
            {
                return new BDictionary( (IDictionary<BString, BElement>)ReadDictionary(typeof(BString), typeof(BElement) ) );
            }
            else if (type == typeof(object) || type == typeof(BElement) )
            {
                switch ( (char)PeekByte() )
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':

                        return new BString(ReadByteString() );

                    case 'i':

                        return new BNumber(ReadInteger() );

                    case 'l':

                        return new BList( (IList<BElement>)ReadList(typeof(BElement) ) );

                    case 'd':

                        return new BDictionary( (IDictionary<BString, BElement>)ReadDictionary(typeof(BString), typeof(BElement) ) );
                }
            }
            else if (ReadObjectHandler != null)
            {
                var value = ReadObjectHandler(type);

                if (value != null)
                {
                    return value;
                }
            }

            throw new BencodeException("Invalid type.");
        }

        /// <exception cref="BencodeException"></exception>
        /// 
        public string ReadString()
        {
            return Encoding.UTF8.GetString(ReadByteString() );
        }

        /// <exception cref="BencodeException"></exception>
        /// 
        public byte[] ReadByteString()
		{
            var digits = ReadUntil(':');

            if (digits != "" && int.TryParse(digits, out var length) && length >= 0)
            {
                return ReadBytes(length);
            }

            throw new BencodeException("Invalid byte string.");
        }

        /// <exception cref="BencodeException"></exception>
        /// 
		public long? ReadInteger()
		{
            if ( (char)ReadByte() == 'i')
            {
                var digits = ReadUntil('e');

                if (digits == "")
                {
                    return null;
                }

                if (long.TryParse(digits, out var value) )
                {
                    return value;
                }
            }

            throw new BencodeException("Invalid integer.");
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public IList ReadList()
        {
            return ReadList(typeof(BElement) );
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public IList<T> ReadList<T>()
        {
            return (IList<T>)ReadList(typeof(T) );
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public IList ReadList(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type) );
            }

            if ( (char)ReadByte() == 'l')
            {
                var value = (IList)Activator.CreateInstance( typeof(List<>).MakeGenericType(type) );

                while ( (char)PeekByte() != 'e')
                {
                    value.Add(ReadObject(type) );
                }

                ReadByte();

                return value;
            }

            throw new BencodeException("Invalid list.");
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public IDictionary ReadDictionary()
        {
            return ReadDictionary(typeof(BString), typeof(BElement) );
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public IDictionary<TKey, TValue> ReadDictionary<TKey, TValue>()
        {
            return (IDictionary<TKey, TValue>)ReadDictionary(typeof(TKey), typeof(TValue) );
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public IDictionary ReadDictionary(Type typeKey, Type typeValue)
        {
            if (typeKey == null)
            {
                throw new ArgumentNullException(nameof(typeKey) );
            }

            if (typeValue == null)
            {
                throw new ArgumentNullException(nameof(typeValue) );
            }

            if ( (char)ReadByte() == 'd')
            {
                var value = (IDictionary)Activator.CreateInstance( typeof(SortedDictionary<,>).MakeGenericType(typeKey, typeValue) );

                while ( (char)PeekByte() != 'e')
                {
                    value.Add(ReadObject(typeKey), ReadObject(typeValue) );
                }

                ReadByte();

                return value;
            }

            throw new BencodeException("Invalid dictionary.");
        }

        public void Dispose()
        {
            stream.Dispose();
        }
    }
}