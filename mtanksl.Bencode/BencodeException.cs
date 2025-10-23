using System;

namespace mtanksl.Bencode
{
    public class BencodeException : Exception
    {
        public BencodeException()
        {
        }

        public BencodeException(string message) : base(message)
        {
        }
    }
}