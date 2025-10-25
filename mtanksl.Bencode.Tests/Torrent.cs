using mtanksl.Bencode.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mtanksl.Bencode.Tests
{
    public class Torrent : IBencodeSerializable
    {
        public string Announce { get; set; }

        public List<List<string>> AnnounceLists { get; set; }

        public string Comment { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public string Encoding { get; set; }

        public TorrentInfo Info { get; set; }

        public void Read(BencodeReader reader)
        {
            var value = reader.ReadObject<BDictionary>();

            Announce = (string)value["announce"];

            var annouceLists = (BList)value["announce-list"];

            if (annouceLists != null)
            {
                AnnounceLists = annouceLists.Value
                    .Cast<BList>()
                    .Select(item => item.Value
                        .Cast<BString>()
                        .Select(item2 => (string)item2)
                        .ToList() )
                    .ToList();
            }

            Comment = (string)value["comment"];
                
            CreatedBy = (string)value["created by"];

            var creationDate = (long?)value["creation date"];

            if (creationDate != null)
            {
                CreationDate = DateTime.UnixEpoch.AddSeconds(creationDate.Value);
            }

            Encoding = (string)value["encoding"];

            var info = (BDictionary)value["info"];

            if (info != null)
            {
                Info = new TorrentInfo()
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
                    Info.Files = files.Value
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
        }

        public void Write(BencodeWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    public class TorrentInfo
    {
        public long PieceLength { get; set; }

        public byte[] Pieces { get; set; }

        public long? Private { get; set; }

        public string Name { get; set; }

        public long? Length { get; set; }

        public string MD5Sum { get; set; }

        public List<TorrentInfoFile> Files { get; set; }
    }

    public class TorrentInfoFile
    {
        public long Length { get; set; }

        public string MD5Sum { get; set; }
        
        public List<string> Path { get; set; }
    }
}