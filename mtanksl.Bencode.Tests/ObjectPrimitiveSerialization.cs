using System.Collections.Generic;

namespace mtanksl.Bencode.Tests
{
    [TestClass]
    public sealed class ObjectPrimitiveSerialization
    {
        [TestMethod]
        public void TestEmptyString()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectPrimitive() { String = "" } );

            Assert.AreEqual("d6:String0:e", bencode);
        }

        [TestMethod]
        public void TestString()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectPrimitive() { String = "Hello World" } );

            Assert.AreEqual("d6:String11:Hello Worlde", bencode);
        }

        [TestMethod]
        public void TestUTF8String()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectPrimitive() { String = "猫" } );

            Assert.AreEqual("d6:String3:猫e", bencode);
        }
        
        [TestMethod]
        public void TestByteArray()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectPrimitive() { ByteArray = new byte[] { 0xE7, 0x8C, 0xAB } } );

            Assert.AreEqual("d9:ByteArray3:猫e", bencode);
        }

        [TestMethod]
        public void TestInteger()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectPrimitive() { Integer = 9223372036854775807L } );
            
            Assert.AreEqual("d7:Integeri9223372036854775807ee", bencode);
        }

        [TestMethod]
        public void TestList()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectPrimitive() { List = new List<object>() { "Hello World", 9223372036854775807L } } );

            Assert.AreEqual("d4:Listl11:Hello Worldi9223372036854775807eee", bencode);
        }

        [TestMethod]
        public void TestDictionary()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectPrimitive() { Dictionary = new SortedDictionary<string, object>() { ["Hello World"] = 9223372036854775807L } } );

            Assert.AreEqual("d10:Dictionaryd11:Hello Worldi9223372036854775807eee", bencode);
        }

        [TestMethod]
        public void TestListOfList()
        {
            var bencode = BencodeConvert.SerializeObject(new TestObjectPrimitive() { ListOfList = new List<List<object>>() { new List<object>() { "Hello World", 9223372036854775807L } } } );
            
            Assert.AreEqual("d10:ListOfListll11:Hello Worldi9223372036854775807eeee", bencode);
        }
    }
}