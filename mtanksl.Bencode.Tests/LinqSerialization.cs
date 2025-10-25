using mtanksl.Bencode.Linq;

namespace mtanksl.Bencode.Tests
{
    [TestClass]
    public sealed class LinqSerialization
    {
        [TestMethod]
        public void TestEmptyString()
        {
            var bencode = BencodeConvert.SerializeObject(new BString("") );
       
            Assert.AreEqual("0:", bencode);
        }

        [TestMethod]
        public void TestString()
        {
            var bencode = BencodeConvert.SerializeObject(new BString("Hello World") );
       
            Assert.AreEqual("11:Hello World", bencode);
        }

        [TestMethod]
        public void TestUTF8String()
        {
            var bencode = BencodeConvert.SerializeObject(new BString("猫") );
       
            Assert.AreEqual("3:猫", bencode);
        }
        
        [TestMethod]
        public void TestByteArray()
        {
            var bencode = BencodeConvert.SerializeObject(new BString(new byte[] { 0xE7, 0x8C, 0xAB } ) );

            Assert.AreEqual("3:猫", bencode);
        }

        [TestMethod]
        public void TestInteger()
        {
            var bencode = BencodeConvert.SerializeObject(new BNumber(9223372036854775807L) );

            Assert.AreEqual("i9223372036854775807e", bencode);
        }

        [TestMethod]
        public void TestList()
        {
            var bencode = BencodeConvert.SerializeObject(new BList(new BString("Hello World"), new BNumber(9223372036854775807L) ) );

            Assert.AreEqual("l11:Hello Worldi9223372036854775807ee", bencode);
        }

        [TestMethod]
        public void TestDictionary()
        {
            var bencode = BencodeConvert.SerializeObject(new BDictionary(new BPair(new BString("Hello World"), new BNumber(9223372036854775807L) ) ) );

            Assert.AreEqual("d11:Hello Worldi9223372036854775807ee", bencode);
        }

        [TestMethod]
        public void TestListOfList()
        {
            var bencode = BencodeConvert.SerializeObject(new BList(new BList(new BString("Hello World"), new BNumber(9223372036854775807L) ) ) );

            Assert.AreEqual("ll11:Hello Worldi9223372036854775807eee", bencode);
        }

        [TestMethod]
        public void TestLinq()
        {
            var value = 
                new BDictionary(
                    new BPair("key 1", ""),
                    new BPair("key 3", 9223372036854775807L),
                    new BPair("key 2", "Hello World"),
                    new BPair("key 4", new BList("", "Hello World", 9223372036854775807L) ) );

            var benconde = BencodeConvert.SerializeObject(value);

            Assert.AreEqual("d5:key 10:5:key 211:Hello World5:key 3i9223372036854775807e5:key 4l0:11:Hello Worldi9223372036854775807eee", benconde);
        }
    }
}