using mtanksl.Bencode.Linq;

namespace mtanksl.Bencode.Tests
{
    [TestClass]
    public sealed class LinqDeserialization
    {
        [TestMethod]
        public void TestEmptyString()
        {
            var value = BencodeConvert.DeserializeObject<BString>("0:");

            Assert.AreEqual("", (string)value);
        }

        [TestMethod]
        public void TestString()
        {
            var value = BencodeConvert.DeserializeObject<BString>("11:Hello World");

            Assert.AreEqual("Hello World", (string)value);
        }

        [TestMethod]
        public void TestInteger()
        {
            var value = BencodeConvert.DeserializeObject<BNumber>("i9223372036854775807e");

            Assert.AreEqual(9223372036854775807L, (long)value);
        }

        [TestMethod]
        public void TestList()
        {
            var value = BencodeConvert.DeserializeObject<BList>("l11:Hello Worldi9223372036854775807ee");

            Assert.AreEqual(2, value.Count);

            Assert.AreEqual("Hello World", (string)value[0] );

            Assert.AreEqual(9223372036854775807L, (long)value[1] );
        }

        [TestMethod]
        public void TestDictionary()
        {
            var value = BencodeConvert.DeserializeObject<BDictionary>("d11:Hello Worldi9223372036854775807ee");

            Assert.AreEqual(1, value.Count);
            
            Assert.AreEqual(9223372036854775807L, (long)value["Hello World"] );
        }

        [TestMethod]
        public void TestLinq()
        {
            var value = (BElement)BencodeConvert.DeserializeObject("d5:key 10:5:key 211:Hello World5:key 3i9223372036854775807e5:key 4l0:11:Hello Worldi9223372036854775807eee");

            Assert.AreEqual("Hello World", (string)value["key 4"][1] );
        }
    }
}