using System.Collections.Generic;

namespace mtanksl.Bencode.Tests
{
    [TestClass]
    public sealed class PrimitiveDeserialization
    {
        [TestMethod]
        public void TestString()
        {
            var value = BencodeConvert.DeserializeObject<string>("11:Hello World"); // "Hello World"

            Assert.AreEqual("Hello World", value);
        }

        [TestMethod]
        public void TestInteger()
        {
            var value = BencodeConvert.DeserializeObject<long>("i9223372036854775807e"); // 9223372036854775807L

            Assert.AreEqual(9223372036854775807L, value);
        }

        [TestMethod]
        public void TestList()
        {
            var value = BencodeConvert.DeserializeObject<List<object>>("l11:Hello Worldi9223372036854775807ee"); // new List<object>() { "Hello World", 9223372036854775807L }

            Assert.AreEqual(value.Count, 2);

            Assert.AreEqual(value[0], "Hello World");

            Assert.AreEqual(value[1], 9223372036854775807L);
        }

        [TestMethod]
        public void TestDictionary()
        {
            var value = BencodeConvert.DeserializeObject<Dictionary<object, object>>("d11:Hello Worldi9223372036854775807ee"); // new Dictionary<object, object>() { ["Hello World"] = 9223372036854775807L } 

            Assert.AreEqual(value.Count, 1);
            
            Assert.AreEqual(value["Hello World"], 9223372036854775807L);
        }
    }
}