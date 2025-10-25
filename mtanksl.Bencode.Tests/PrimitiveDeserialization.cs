using mtanksl.Bencode.Converters;
using mtanksl.Bencode.Linq;
using System;
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
        public void TestUTF8String()
        {
            var value = BencodeConvert.DeserializeObject<string>("3:猫");

            Assert.AreEqual("猫", value);
        }

        [TestMethod]
        public void TestByteArray()
        {
            var value = BencodeConvert.DeserializeObject<byte[]>("3:猫");

            var expected = new byte[] { 0xE7, 0x8C, 0xAB };

            Assert.AreEqual(expected.Length, value.Length);

            for (int i = 0; i < value.Length; i++)
            {
                Assert.AreEqual(expected[i], value[i] );
            }
        }

        [TestMethod]
        public void TestInteger()
        {
            var value = BencodeConvert.DeserializeObject<long>("i9223372036854775807e");

            Assert.AreEqual(9223372036854775807L, value);
        }

        [TestMethod]
        public void TestNullableInteger()
        {
            // This scenario is ambiguous per the specification

            var value = BencodeConvert.DeserializeObject<long?>("ie");

            Assert.AreEqual(null, value);
        }

        [TestMethod]
        public void TestDateTime()
        {
            var bencode = BencodeConvert.DeserializeObject<DateTime>("i1726164800e", new BencodeSerializerSettings() { Converters = new List<BencodeConverter>() { new DateTimeConverter() } } );

            Assert.AreEqual(new DateTime(2024, 09, 12, 18, 13, 20, DateTimeKind.Utc), bencode);
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
        public void TestDictionary()
        {
            var value = BencodeConvert.DeserializeObject<SortedDictionary<string, object>>("d11:Hello Worldi9223372036854775807ee");

            Assert.AreEqual(1, value.Count);
            
            Assert.AreEqual(9223372036854775807L, (long)(BNumber)value["Hello World"] );
        }

        [TestMethod]
        public void TestListOfList()
        {
            var value = BencodeConvert.DeserializeObject<List<List<object>>>("ll11:Hello Worldi9223372036854775807eee");

            Assert.AreEqual(1, value.Count);

            Assert.AreEqual("Hello World", (string)(BString)value[0][0] );

            Assert.AreEqual(9223372036854775807L, (long)(BNumber)value[0][1] );
        }
    }
}