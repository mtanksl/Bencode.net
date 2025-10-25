using System;

namespace mtanksl.Bencode
{
    public abstract class BencodeConverter
    {
        public abstract bool CanConvert(Type type);

        public abstract object Read(BencodeReader reader, Type type, BencodeSerializer serializer);

        public abstract void Write(BencodeWriter writer, object value, Type type, BencodeSerializer serializer);
    }
}