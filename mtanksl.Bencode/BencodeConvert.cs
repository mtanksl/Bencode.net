using mtanksl.Bencode.Linq;
using System;
using System.IO;
using System.Text;

namespace mtanksl.Bencode
{
    public static class BencodeConvert
    {
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// 
        public static string SerializeObject(object value, BencodeSerializerSettings settings = null)
        {
            if (value == null)
			{
                throw new ArgumentNullException(nameof(value) );
            }

            return SerializeObject(value, value.GetType(), settings);
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// 
        public static string SerializeObject(object value, Type type, BencodeSerializerSettings settings = null)
        {
            if (type == null)
			{
                throw new ArgumentNullException(nameof(type) );
            }

            using (var stream = new MemoryStream() )
            {
                using (var writer = new BencodeWriter(stream) )
                {
                    var serializer = new BencodeSerializer(settings);

                    serializer.Serialize(writer, value, type);

                    return Encoding.UTF8.GetString(stream.ToArray() );
                }
            }
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
		public static object DeserializeObject(string bencode, BencodeSerializerSettings settings = null)
        {
            return DeserializeObject(bencode, typeof(BElement), settings);
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public static T DeserializeObject<T>(string bencode, BencodeSerializerSettings settings = null)
        {
            return (T)DeserializeObject(bencode, typeof(T), settings);
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public static object DeserializeObject(string bencode, Type type, BencodeSerializerSettings settings = null)
        {
            if (bencode == null)
			{
				throw new ArgumentNullException(nameof(bencode) );
			}

            if (type == null)
            {
				throw new ArgumentNullException(nameof(type) );
            }

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(bencode) ) )
            {
                using (var reader = new BencodeReader(stream) )
                {
                    var serializer = new BencodeSerializer(settings);

                    return serializer.Deserialize(reader, type);
                }
            }
        }
    }
}