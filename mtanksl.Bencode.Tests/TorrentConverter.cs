using mtanksl.Bencode.Linq;
using System;
using System.Linq;

namespace mtanksl.Bencode.Tests
{
    public class TorrentConverter : BencodeConverter
    {
        public override bool CanConvert(Type type)
        {
            return type == typeof(Torrent);
        }

        public override object Read(BencodeReader reader, Type type, BencodeSerializer serializer)
        {
            if (type == typeof(Torrent) )
            {
                var torrent = new Torrent();

                var value = reader.ReadObject<BDictionary>();

                torrent.Announce = (string)value["announce"];

                var annouceLists = (BList)value["announce-list"];

                if (annouceLists != null)
                {
                    torrent.AnnounceLists = annouceLists.Value
                        .Cast<BList>()
                        .Select(item => item.Value
                            .Cast<BString>()
                            .Select(item2 => (string)item2)
                            .ToList() )
                        .ToList();
                }

                torrent.Comment = (string)value["comment"];

                torrent.CreatedBy = (string)value["created by"];

                var creationDate = (long?)value["creation date"];

                if (creationDate != null)
                {
                    torrent.CreationDate = DateTime.UnixEpoch.AddSeconds(creationDate.Value);
                }

                torrent.Encoding = (string)value["encoding"];

                var info = (BDictionary)value["info"];

                if (info != null)
                {
                    torrent.Info = new TorrentInfo()
                    {
                        PieceLength = (long)info["piece length"],

                        Pieces = (byte[] )info["pieces"],

                        Private = (long?)info["private"],

                        Name = (string)info["name"],

                        Length = (long?)info["length"],

                        MD5Sum = (string)info["md5sum"]
                    };

                    var files = (BList)info["files"];

                    if (files != null)
                    {
                        torrent.Info.Files = files.Value
                            .Cast<BDictionary>()
                            .Select(item =>
                            {
                                var path = (BList)item["path"];

                                return new TorrentInfoFile() 
                                {
                                    Length = (long)item["length"],

                                    MD5Sum = (string)item["md5sum"],
                            
                                    Path = path == null ? null : path.Value
                                        .Cast<BString>()
                                        .Select(item2 => (string)item2)
                                        .ToList() };
                            } )
                            .ToList();
                    }
                }

                return torrent;
            }

            throw new NotImplementedException();
        }

        public override void Write(BencodeWriter writer, object value, Type type, BencodeSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}