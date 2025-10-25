using mtanksl.Bencode.Linq;

namespace mtanksl.Bencode.Tests
{
    [TestClass]
    public sealed class ObjectLinqSerialization
    {
        [TestMethod]
        public void TestEmptyString()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectLinq() { String = new BString("") } );

            Assert.AreEqual("d6:String0:e", bencode);
        }

        [TestMethod]
        public void TestString()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectLinq() { String = new BString("Hello World") } );

            Assert.AreEqual("d6:String11:Hello Worlde", bencode);
        }

        [TestMethod]
        public void TestUTF8String()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectLinq() { String = new BString("猫") } );

            Assert.AreEqual("d6:String3:猫e", bencode);
        }
        
        [TestMethod]
        public void TestByteArray()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectLinq() { ByteArray = new BString(new byte[] { 0xE7, 0x8C, 0xAB } ) } );

            Assert.AreEqual("d9:ByteArray3:猫e", bencode);
        }

        [TestMethod]
        public void TestInteger()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectLinq() { Integer = new BNumber(9223372036854775807L) } );
            
            Assert.AreEqual("d7:Integeri9223372036854775807ee", bencode);
        }

        [TestMethod]
        public void TestList()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectLinq() { List = new BList(new BString("Hello World"), new BNumber(9223372036854775807L) ) } );

            Assert.AreEqual("d4:Listl11:Hello Worldi9223372036854775807eee", bencode);
        }

        [TestMethod]
        public void TestDictionary()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectLinq() { Dictionary = new BDictionary(new BPair(new BString("Hello World"), new BNumber(9223372036854775807L) ) ) } );

            Assert.AreEqual("d10:Dictionaryd11:Hello Worldi9223372036854775807eee", bencode);
        }

        [TestMethod]
        public void TestListOfList()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectLinq() { ListOfList = new BList(new BList(new BString("Hello World"), new BNumber(9223372036854775807L) ) ) } );
            
            Assert.AreEqual("d10:ListOfListll11:Hello Worldi9223372036854775807eeee", bencode);
        }
    }
}