using mtanksl.Bencode.Linq;

namespace mtanksl.Bencode.Tests
{
    [BencodeObject]
    public class TestObjectLinq
    {
        [BencodeProperty("String")]
        public BString String { get; set; }

        [BencodeProperty("ByteArray")]
        public BString ByteArray { get; set; }

        [BencodeProperty("Integer")]
        public BNumber Integer { get; set; }

        [BencodeProperty("List")]
        public BList List { get; set; }

        [BencodeProperty("Dictionary")]
        public BDictionary Dictionary { get; set; }

        [BencodeProperty("ListOfList")]
        public BList ListOfList { get; set; }
    }
}