using mtanksl.Bencode.Linq;

namespace mtanksl.Bencode.Tests
{
    [TestClass]
    public sealed class ObjectLinqDeserialization
    {
        [TestMethod]
        public void TestEmptyString()
        {
            var value = BencodeConvert.DeserializeObject<TestObjectLinq>("d6:String0:e");
       
            Assert.AreEqual("", (string)value.String);
        }

        [TestMethod]
        public void TestString()
        {
            var value = BencodeConvert.DeserializeObject<TestObjectLinq>("d6:String11:Hello Worlde");

            Assert.AreEqual("Hello World", (string)value.String);
        }


        
        [TestMethod]
        public void TestByteArray()
        {
            var value = BencodeConvert.DeserializeObject<TestObjectLinq>("d9:ByteArray3:猫e");

            var expected = new byte[] { 0xE7, 0x8C, 0xAB };
            
            Assert.AreEqual(expected.Length, ( (byte[] )value.ByteArray).Length);

            for (int i = 0; i < ( (byte[] )value.ByteArray).Length; i++)
            {
                Assert.AreEqual(expected[i], ( (byte[] )value.ByteArray)[i] );
            }
        }

        [TestMethod]
        public void TestInteger()
        {
            var value = BencodeConvert.DeserializeObject<TestObjectLinq>("d7:Integeri9223372036854775807ee");

            Assert.AreEqual(9223372036854775807L, (long)value.Integer);
        }

        [TestMethod]
        public void TestList()
        {
            var value = BencodeConvert.DeserializeObject<TestObjectLinq>("d4:Listl11:Hello Worldi9223372036854775807eee");

            Assert.AreEqual(2, value.List.Count);

            Assert.AreEqual("Hello World", (string)value.List[0] );

            Assert.AreEqual(9223372036854775807L, (long)value.List[1] );
        }

        [TestMethod]
        public void TestDictionary()
        {
            var value = BencodeConvert.DeserializeObject<TestObjectLinq>("d10:Dictionaryd11:Hello Worldi9223372036854775807eee");

            Assert.AreEqual(1, value.Dictionary.Count);
            
            Assert.AreEqual(9223372036854775807L, (long)value.Dictionary["Hello World"] );
        }

        [TestMethod]
        public void TestListOfList()
        {
            var value = BencodeConvert.DeserializeObject<TestObjectLinq>("d10:ListOfListll11:Hello Worldi9223372036854775807eeee");

            Assert.AreEqual(1, value.ListOfList.Count);

            Assert.AreEqual("Hello World", (string)value.ListOfList[0][0] );

            Assert.AreEqual(9223372036854775807L, (long)value.ListOfList[0][1] );
        }
    }
}