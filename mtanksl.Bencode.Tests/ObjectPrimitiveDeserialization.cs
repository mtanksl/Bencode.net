using mtanksl.Bencode.Linq;

namespace mtanksl.Bencode.Tests
{
    [TestClass]
    public sealed class ObjectPrimitiveDeserialization
    {
        [TestMethod]
        public void TestEmptyString()
        {
            var value = BencodeConvert.DeserializeObject<TestObjectPrimitive>("d6:String0:e");
       
            Assert.AreEqual("", value.String);
        }

        [TestMethod]
        public void TestString()
        {
            var value = BencodeConvert.DeserializeObject<TestObjectPrimitive>("d6:String11:Hello Worlde");

            Assert.AreEqual("Hello World", value.String);
        }

        [TestMethod]
        public void TestUTF8String()
        {
            var value = BencodeConvert.DeserializeObject<TestObjectPrimitive>("d6:String3:猫e");

            Assert.AreEqual("猫", value.String);
        }
        
        [TestMethod]
        public void TestByteArray()
        {
            var value = BencodeConvert.DeserializeObject<TestObjectPrimitive>("d9:ByteArray3:猫e");

            var expected = new byte[] { 0xE7, 0x8C, 0xAB };
            
            Assert.AreEqual(expected.Length, value.ByteArray.Length);

            for (int i = 0; i < value.ByteArray.Length; i++)
            {
                Assert.AreEqual(expected[i], value.ByteArray[i] );
            }
        }

        [TestMethod]
        public void TestInteger()
        {
            var value = BencodeConvert.DeserializeObject<TestObjectPrimitive>("d7:Integeri9223372036854775807ee");

            Assert.AreEqual(9223372036854775807L, value.Integer);
        }

        [TestMethod]
        public void TestList()
        {
            var value = BencodeConvert.DeserializeObject<TestObjectPrimitive>("d4:Listl11:Hello Worldi9223372036854775807eee");

            Assert.AreEqual(2, value.List.Count);

            Assert.AreEqual("Hello World", (string)(BString)value.List[0] );

            Assert.AreEqual(9223372036854775807L, (long)(BNumber)value.List[1] );
        }

        [TestMethod]
        public void TestDictionary()
        {
            var value = BencodeConvert.DeserializeObject<TestObjectPrimitive>("d10:Dictionaryd11:Hello Worldi9223372036854775807eee");

            Assert.AreEqual(1, value.Dictionary.Count);
            
            Assert.AreEqual(9223372036854775807L, (long)(BNumber)value.Dictionary["Hello World"] );
        }

        [TestMethod]
        public void TestListOfList()
        {
            var value = BencodeConvert.DeserializeObject<TestObjectPrimitive>("d10:ListOfListll11:Hello Worldi9223372036854775807eeee");

            Assert.AreEqual(1, value.ListOfList.Count);

            Assert.AreEqual("Hello World", (string)(BString)value.ListOfList[0][0] );

            Assert.AreEqual(9223372036854775807L, (long)(BNumber)value.ListOfList[0][1] );
        }
    }
}