using System.Collections.Generic;

namespace mtanksl.Bencode.Tests
{
    [TestClass]
    public sealed class PrimitiveSerialization
    {
        [TestMethod]
        public void TestEmptyString()
        {
            var bencode = BencodeConvert.SerializeObject("");
       
            Assert.AreEqual("0:", bencode);
        }

        [TestMethod]
        public void TestString()
        {
            var bencode = BencodeConvert.SerializeObject("Hello World");
       
            Assert.AreEqual("11:Hello World", bencode);
        }

        [TestMethod]
        public void TestInteger()
        {
            var bencode = BencodeConvert.SerializeObject(9223372036854775807L);

            Assert.AreEqual("i9223372036854775807e", bencode);
        }

        [TestMethod]
        public void TestList()
        {
            var bencode = BencodeConvert.SerializeObject(new List<object>() { "Hello World", 9223372036854775807L } );

            Assert.AreEqual("l11:Hello Worldi9223372036854775807ee", bencode);
        }

        [TestMethod]
        public void TestDictionary()
        {
            var bencode = BencodeConvert.SerializeObject(new SortedDictionary<string, object>() { ["Hello World"] = 9223372036854775807L } );

            Assert.AreEqual("d11:Hello Worldi9223372036854775807ee", bencode);
        }
    }
}