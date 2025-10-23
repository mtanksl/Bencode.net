using mtanksl.Bencode.Linq;
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

            Assert.AreEqual("Hello World", (string)(BString)value[0] );

            Assert.AreEqual(9223372036854775807L, (long)(BNumber)value[1] );
        }

        [TestMethod]
        public void TestListOfBElement()
        {
            var value = BencodeConvert.DeserializeObject<List<BElement>>("l5:Hello5:Worlde");

            Assert.AreEqual(2, value.Count);

            Assert.AreEqual("Hello", (string)(BString)value[0] );

            Assert.AreEqual("World", (string)(BString)value[1] );
        }

        [TestMethod]
        public void TestListOfBString()
        {
            var value = BencodeConvert.DeserializeObject<List<BString>>("l5:Hello5:Worlde");

            Assert.AreEqual(2, value.Count);

            Assert.AreEqual("Hello", (string)value[0] );

            Assert.AreEqual("World", (string)value[1] );
        }

        [TestMethod]
        public void TestListOfString()
        {
            var value = BencodeConvert.DeserializeObject<List<string>>("l5:Hello5:Worlde");

            Assert.AreEqual(2, value.Count);

            Assert.AreEqual("Hello", value[0] );

            Assert.AreEqual("World", value[1] );
        }

        [TestMethod]
        public void TestDictionary()
        {
            var value = BencodeConvert.DeserializeObject<SortedDictionary<string, object>>("d11:Hello Worldi9223372036854775807ee");

            Assert.AreEqual(1, value.Count);
            
            Assert.AreEqual(9223372036854775807L, (long)(BNumber)value["Hello World"] );
        }

        [TestMethod]
        public void TestDictionaryOfBElement()
        {
            var value = BencodeConvert.DeserializeObject<SortedDictionary<string, BElement>>("d11:Hello Worldi9223372036854775807ee");

            Assert.AreEqual(1, value.Count);
            
            Assert.AreEqual(9223372036854775807L, (long)(BNumber)value["Hello World"] );
        }

        [TestMethod]
        public void TestDictionaryOfBNumber()
        {
            var value = BencodeConvert.DeserializeObject<SortedDictionary<string, BNumber>>("d11:Hello Worldi9223372036854775807ee");

            Assert.AreEqual(1, value.Count);
            
            Assert.AreEqual(9223372036854775807L, (long)value["Hello World"] );
        }

        [TestMethod]
        public void TestDictionaryOfLong()
        {
            var value = BencodeConvert.DeserializeObject<SortedDictionary<string, long>>("d11:Hello Worldi9223372036854775807ee");

            Assert.AreEqual(1, value.Count);

            Assert.AreEqual(9223372036854775807L, value["Hello World"] );
        }
    }
}