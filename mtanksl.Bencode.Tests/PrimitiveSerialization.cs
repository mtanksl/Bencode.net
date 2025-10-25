using mtanksl.Bencode.Converters;
using System;
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
        public void TestUTF8String()
        {
            var bencode = BencodeConvert.SerializeObject("猫");
       
            Assert.AreEqual("3:猫", bencode);
        }
        
        [TestMethod]
        public void TestByteArray()
        {
            var bencode = BencodeConvert.SerializeObject(new byte[] { 0xE7, 0x8C, 0xAB } );

            Assert.AreEqual("3:猫", bencode);
        }

        [TestMethod]
        public void TestInteger()
        {
            var bencode = BencodeConvert.SerializeObject(9223372036854775807L);

            Assert.AreEqual("i9223372036854775807e", bencode);
        }

        [TestMethod]
        public void TestNullableInteger()
        {
            // This scenario is ambiguous per the specification

            var bencode = BencodeConvert.SerializeObject(null, typeof(long?) );

            Assert.AreEqual("ie", bencode);
        }

        [TestMethod]
        public void TestDateTime()
        {
            var bencode = BencodeConvert.SerializeObject(new DateTime(2024, 09, 12, 18, 13, 20, DateTimeKind.Utc), new BencodeSerializerSettings() { Converters = new List<BencodeConverter>() { new DateTimeConverter() } } );

            Assert.AreEqual("i1726164800e", bencode);
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

        [TestMethod]
        public void TestListOfList()
        {
            var bencode = BencodeConvert.SerializeObject(new List<List<object>>() { new List<object>() { "Hello World", 9223372036854775807L } } );

            Assert.AreEqual("ll11:Hello Worldi9223372036854775807eee", bencode);
        }
    }
}