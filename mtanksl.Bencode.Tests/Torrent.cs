using System;
using System.Collections.Generic;

namespace mtanksl.Bencode.Tests
{
    [BencodeObject(ItemConverterType = typeof(TorrentConverter))]
    public class Torrent
    {
        public string Announce { get; set; }

        public List<List<string>> AnnounceLists { get; set; }

        public string Comment { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public string Encoding { get; set; }

        public TorrentInfo Info { get; set; }
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