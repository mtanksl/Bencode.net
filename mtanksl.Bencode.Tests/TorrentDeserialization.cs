using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mtanksl.Bencode.Tests
{
    [TestClass]
    public sealed class TorrentDeserialization
    {
        [TestMethod]
        public void Test()
        {
            var torrentFile = File.ReadAllText("ubuntu-22.04.5-desktop-amd64.iso.torrent", Encoding.Latin1);

            var value = BencodeConvert.DeserializeObject<Dictionary<string ,object>>(torrentFile);

            Assert.AreEqual("https://torrent.ubuntu.com/announce", value["announce"] );

            Assert.AreEqual(2, ( (List<object>)value["announce-list"] ).Count);

            Assert.AreEqual(1726164800L, value["creation date"] );

            Assert.AreEqual("Ubuntu CD releases.ubuntu.com", value["comment"] );

            Assert.AreEqual("mktorrent 1.1", value["created by"] );

            Assert.AreEqual(4, ( (Dictionary<object, object>)value["info"] ).Count);
        }
    }
}