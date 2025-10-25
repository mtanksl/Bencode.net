using System;

namespace mtanksl.Bencode
{
    public interface IBencodeSerializable
    {
        void Read(BencodeReader reader, Type type, BencodeSerializer serializer);
        
        void Write(BencodeWriter writer, Type type, BencodeSerializer serializer);
    }
}