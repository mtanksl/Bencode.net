using mtanksl.Bencode.Linq;
using System.IO;
using System.Text;

namespace mtanksl.Bencode.Tests
{
    [TestClass]
    public sealed class TorrentDeserialization
    {
        [TestMethod]
        public void TestTorrent()
        {
            var torrent = File.ReadAllText("ubuntu-22.04.5-desktop-amd64.iso.torrent", Encoding.Latin1);

            var value = (BElement)BencodeConvert.DeserializeObject(torrent);

            Assert.AreEqual("https://torrent.ubuntu.com/announce", (string)value["announce"] );

            Assert.AreEqual(2, ( (BList)value["announce-list"] ).Count);

            Assert.AreEqual("Ubuntu CD releases.ubuntu.com", (string)value["comment"] );

            Assert.AreEqual("mktorrent 1.1", (string)value["created by"] );

            Assert.AreEqual(1726164800L, (long)value["creation date"] );

            Assert.AreEqual(4, ( (BDictionary)value["info"] ).Count);
        }
    }
}