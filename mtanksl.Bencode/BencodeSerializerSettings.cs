namespace mtanksl.Bencode
{
    public class BencodeSerializerSettings
    {
        public static readonly BencodeSerializerSettings Default = new BencodeSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

        public NullValueHandling NullValueHandling { get; set; }
    }
}