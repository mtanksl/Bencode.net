using mtanksl.Bencode.Linq;
using System;
using System.IO;

namespace mtanksl.Bencode.Tests
{
    [TestClass]
    public sealed class TorrentDeserialization
    {
        [TestMethod]
        public void TestTorrentLinq()
        {
            using (var stream = File.OpenRead("ubuntu-22.04.5-desktop-amd64.iso.torrent") )
            {
                using (var reader = new BencodeReader(stream) )
                {
                    var serializer = new BencodeSerializer();

                    var value = (BElement)serializer.Deserialize(reader);

                    Assert.AreEqual("https://torrent.ubuntu.com/announce", (string)value["announce"] );

                    Assert.AreEqual(2, ( (BList)value["announce-list"] ).Count);

                    Assert.AreEqual("Ubuntu CD releases.ubuntu.com", (string)value["comment"] );

                    Assert.AreEqual("mktorrent 1.1", (string)value["created by"] );

                    Assert.AreEqual(1726164800L, (long)value["creation date"] );

                    Assert.AreEqual(4, ( (BDictionary)value["info"] ).Count);
                }
            }
        }

        [TestMethod]
        public void TestTorrentObject()
        {
            using (var stream = File.OpenRead("ubuntu-22.04.5-desktop-amd64.iso.torrent") )
            {
                using (var reader = new BencodeReader(stream) )
                {
                    var serializer = new BencodeSerializer();

                    var value = serializer.Deserialize<Torrent>(reader);

                    Assert.AreEqual("https://torrent.ubuntu.com/announce", value.Announce);

                    Assert.AreEqual(2, value.AnnounceLists.Count);

                    Assert.AreEqual("Ubuntu CD releases.ubuntu.com", value.Comment);

                    Assert.AreEqual("mktorrent 1.1", value.CreatedBy);

                    Assert.AreEqual(new DateTime(2024, 09, 12, 18, 13, 20, DateTimeKind.Utc), value.CreationDate);

                    Assert.AreEqual(262144L, value.Info.PieceLength);

                    Assert.AreEqual(363380L, value.Info.Pieces.Length);

                    Assert.AreEqual(4762707968L, value.Info.Length);

                    Assert.AreEqual("ubuntu-22.04.5-desktop-amd64.iso", value.Info.Name);
                }
            }
        }
    }
}