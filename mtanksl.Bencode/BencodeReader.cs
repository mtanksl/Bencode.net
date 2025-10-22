using System;
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
                throw new BencodeException();
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
                throw new BencodeException();
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
                throw new BencodeException();
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
                    throw new BencodeException();
                }

                if (value == character)
                {
                    break;
                }

                stringBuilder.Append( (char)value);
            }

            return stringBuilder.ToString();
        }

        /// <exception cref="BencodeException"></exception>
        /// 
        public object ReadObject()
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

                    return ReadList();

                case 'd':

                    return ReadDictionary();
            }

            throw new BencodeException();
        }

        /// <exception cref="BencodeException"></exception>
        /// 
        public string ReadString()
		{
            if (int.TryParse(ReadUntil(':'), out var length) && length >= 0)
            {
                if (length > 0)
                {
                    return Read(length);
                }

                return null;
            }

            throw new BencodeException();
        }

        /// <exception cref="BencodeException"></exception>
        /// 
		public long ReadInteger()
		{
            if (Read() == 'i' && long.TryParse(ReadUntil('e'), out var value) )
            {
                return value;
            }

            throw new BencodeException();
        }

        /// <exception cref="BencodeException"></exception>
        /// 
        private List<object> ReadList()
        {
            if (Read() == 'l')
            {
                var value = new List<object>();

                while (Peek() != 'e')
                {
                    value.Add(ReadObject() );
                }

				return value;
            }

            throw new BencodeException();
        }

        /// <exception cref="BencodeException"></exception>
        ///
        private Dictionary<object, object> ReadDictionary()
        {
			if (Read() == 'd')
            {
                var value = new Dictionary<object, object>();

                while (Peek() != 'e')
                {
                    value.Add(ReadObject(), ReadObject() );
                }

				return value;
            }

            throw new BencodeException();
        }

        public void Dispose()
        {
            textReader.Dispose();
        }
    }
}