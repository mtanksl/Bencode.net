using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mtanksl.Bencode
{
    public class BencodeReader : IDisposable
    {
		private TextReader textReader;

        /// <exception cref="ArgumentNullException"></exception>
        /// 
        public BencodeReader(TextReader textReader)
        {
			if (textReader == null)
			{
				throw new ArgumentNullException(nameof(textReader) );
			}

			this.textReader = textReader;
        }

        /// <exception cref="BencodeException"></exception>
        /// 
        private char Peek()
        {
            var value = textReader.Peek();

            if (value == -1)
            {
                throw new BencodeException("End of stream.");
            }

            return (char)value;
        }

        /// <exception cref="BencodeException"></exception>
        /// 
        private char Read()
        {
            var value = textReader.Read();

            if (value == -1)
            {
                throw new BencodeException("End of stream.");
            }

            return (char)value;
        }

        /// <exception cref="BencodeException"></exception>
        /// 
        private string Read(int length)
        {
            var buffer = new char[length];

            if (textReader.ReadBlock(buffer, 0, buffer.Length) != length)
            {
                throw new BencodeException("End of stream.");
            }

            return new string(buffer);
        }

        /// <exception cref="BencodeException"></exception>
        /// 
        private string ReadUntil(char character)
        {
            var stringBuilder = new StringBuilder();

            while (true)
            {
                var value = textReader.Read();

                if (value == -1)
                {
                    throw new BencodeException("End of stream.");
                }

                if (value == character)
                {
                    break;
                }

                stringBuilder.Append( (char)value);
            }

            return stringBuilder.ToString();
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

            if (typeof(IBencodeSerializable).IsAssignableFrom(type) )
            {
                var value = (IBencodeSerializable)Activator.CreateInstance(type);

                value.Read(this);

                return value;
            }
            else if (type == typeof(string) )
            {
                return ReadString();
            }
            else if (type == typeof(sbyte) || type == typeof(byte) || type == typeof(short) || type == typeof(ushort) || type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) )
            {
                return ReadInteger();
            }
            else if (typeof(IList).IsAssignableFrom(type) )
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) )
                {
                    var genericArguments = type.GetGenericArguments();

                    return ReadList(genericArguments[0] );
                }
                else
                {
                    return ReadList(typeof(object) );
                }
            }
            else if (typeof(IDictionary).IsAssignableFrom(type) )
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SortedDictionary<,>) )
                {
                    var genericArguments = type.GetGenericArguments();

                    return ReadDictionary(genericArguments[1] );
                }
                else
                {
                    return ReadDictionary(typeof(object) );
                }
            }
            else
            {
                switch (Peek() )
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

                        return ReadString();

                    case 'i':

                        return ReadInteger();

                    case 'l':

                        return ReadList(typeof(object) );

                    case 'd':

                        return ReadDictionary(typeof(object) );

                    default:

                        throw new BencodeException("Invalid type.");
                }
            }
        }

        /// <exception cref="BencodeException"></exception>
        /// 
        public string ReadString()
		{
            if (int.TryParse(ReadUntil(':'), out var length) && length >= 0)
            {
                return Read(length);
            }

            throw new BencodeException("Invalid string.");
        }

        /// <exception cref="BencodeException"></exception>
        /// 
		public long ReadInteger()
		{
            if (Read() == 'i' && long.TryParse(ReadUntil('e'), out var value) )
            {
                return value;
            }

            throw new BencodeException("Invalid integer.");
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public List<T> ReadList<T>()
        {
            return (List<T>)ReadList(typeof(T) );
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

            if (Read() == 'l')
            {
                var value = (IList)Activator.CreateInstance( typeof(List<>).MakeGenericType(type) );

                while (Peek() != 'e')
                {
                    value.Add(ReadObject(type) );
                }

                Read();

                return value;
            }

            throw new BencodeException("Invalid list.");
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public SortedDictionary<string, T> ReadDictionary<T>()
        {
            return (SortedDictionary<string, T>)ReadDictionary(typeof(T) );
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public IDictionary ReadDictionary(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type) );
            }

            if (Read() == 'd')
            {
                var value = (IDictionary)Activator.CreateInstance( typeof(SortedDictionary<,>).MakeGenericType(typeof(string), type) );

                while (Peek() != 'e')
                {
                    value.Add(ReadString(), ReadObject(type) );
                }

                Read();

                return value;
            }

            throw new BencodeException("Invalid dictionary.");
        }

        public void Dispose()
        {
            textReader.Dispose();
        }
    }
}