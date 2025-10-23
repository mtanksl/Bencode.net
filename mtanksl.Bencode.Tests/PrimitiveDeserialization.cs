using System.Collections.Generic;

namespace mtanksl.Bencode.Tests
{
    [TestClass]
    public sealed class PrimitiveDeserialization
    {
        [TestMethod]
        public void TestEmptyString()
        {
            var value = BencodeConvert.DeserializeObject<string>("0:");

            Assert.AreEqual("", value);
        }

        [TestMethod]
        public void TestString()
        {
            var value = BencodeConvert.DeserializeObject<string>("11:Hello World");

            Assert.AreEqual("Hello World", value);
        }

        [TestMethod]
        public void TestInteger()
        {
            var value = BencodeConvert.DeserializeObject<long>("i9223372036854775807e");

            Assert.AreEqual(9223372036854775807L, value);
        }

        [TestMethod]
        public void TestList()
        {
            var value = BencodeConvert.DeserializeObject<List<object>>("l11:Hello Worldi9223372036854775807ee");

            Assert.AreEqual(2, value.Count);

            Assert.AreEqual("Hello World", value[0] );

            Assert.AreEqual(9223372036854775807L, value[1] );
        }

        [TestMethod]
        public void TestDictionary()
        {
            var value = BencodeConvert.DeserializeObject<SortedDictionary<string, object>>("d11:Hello Worldi9223372036854775807ee");

            Assert.AreEqual(1, value.Count);
            
            Assert.AreEqual(9223372036854775807L, value["Hello World"] );
        }
    }
}