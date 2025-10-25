using System.Collections.Generic;

namespace mtanksl.Bencode.Tests
{
    [BencodeObject]
    public class TestObjectPrimitive
    {
        [BencodeProperty("String")]
        public string String { get; set; }

        [BencodeProperty("ByteArray")]
        public byte[] ByteArray { get; set; }

        [BencodeProperty("Integer")]
        public long? Integer { get; set; }

        [BencodeProperty("List")]
        public List<object> List { get; set; }

        [BencodeProperty("Dictionary")]
        public SortedDictionary<string, object> Dictionary { get; set; }

        [BencodeProperty("ListOfList")]
        public List<List<object>> ListOfList { get; set; }
    }
}