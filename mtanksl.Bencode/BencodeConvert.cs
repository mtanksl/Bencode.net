using mtanksl.Bencode.Linq;
using System;
using System.IO;

namespace mtanksl.Bencode
{
    public static class BencodeConvert
    {
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// 
        public static string SerializeObject(object value)
        {
            if (value == null)
			{
                throw new ArgumentNullException(nameof(value) );
            }

            using (var stringWriter = new StringWriter() )
            {
                using (var writer = new BencodeWriter(stringWriter) )
                {
                    writer.WriteObject(value);

                    return stringWriter.ToString();
                }
            }
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
		public static object DeserializeObject(string bencode)
        {
            return DeserializeObject(bencode, typeof(object) );
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public static T DeserializeObject<T>(string bencode)
        {
            return (T)DeserializeObject(bencode, typeof(T) );
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public static object DeserializeObject(string bencode, Type type)
        {
            if (string.IsNullOrEmpty(bencode) )
			{
				throw new ArgumentNullException(nameof(bencode) );
			}

            if (type == null)
            {
				throw new ArgumentNullException(nameof(type) );
            }

            using (var stringReader = new StringReader(bencode) )
            {
                using (var reader = new BencodeReader(stringReader) )
                {
                    return reader.ReadObject(type);
                }
            }
        }
    }
}