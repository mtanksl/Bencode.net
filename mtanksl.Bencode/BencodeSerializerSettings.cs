using System.Collections.Generic;

namespace mtanksl.Bencode
{
    public class BencodeSerializerSettings
    {
        public List<BencodeConverter> Converters { get; set; }

        public NullValueHandling NullValueHandling { get; set; }
    }
}