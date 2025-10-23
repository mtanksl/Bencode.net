namespace mtanksl.Bencode
{
    public interface IBencodeSerializable
    {
        void Read(BencodeReader reader);
        
        void Write(BencodeWriter writer);
    }
}