using mtanksl.Bencode.Linq;
using System;
using System.Collections;
using System.IO;

namespace mtanksl.Bencode
{
    public class BencodeWriter : IDisposable
    {
        private TextWriter textWriter;

        /// <exception cref="ArgumentNullException"></exception>
        /// 
        public BencodeWriter(TextWriter textWriter)
        {
            if (textWriter == null)
			{
				throw new ArgumentNullException(nameof(textWriter) );
			}

            this.textWriter = textWriter;
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// 
        public void WriteObject(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value) );
            }

            var type = value.GetType();

            if (typeof(IBencodeSerializable).IsAssignableFrom(type) )
            {
                ( (IBencodeSerializable)value).Write(this);
            }
            else if (type == typeof(BString) )
            {
                WriteString( ( (BString)value).Value);
            }
            else if (type == typeof(BNumber) )
            {
                WriteNumber( ( (BNumber)value).Value);
            }
            else if (type == typeof(BList) )
            {
                WriteList( ( (BList)value).Value);
            }
            else if (type == typeof(BDictionary) )
            {
                WriteDictionary( ( (BDictionary)value).Value);
            }
            else if (type == typeof(string) )
            {
                WriteString( (string)value);
            }
            else if (type == typeof(sbyte) || type == typeof(byte) || type == typeof(short) || type == typeof(ushort) || type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) )
            {
                WriteNumber( (long)value);
            }
            else if (typeof(IList).IsAssignableFrom(type) )
            {
                WriteList( (IList)value);
            }
            else if (typeof(IDictionary).IsAssignableFrom(type) )
            {                                
                WriteDictionary( (IDictionary)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public void WriteString(string value)
        {
			if (string.IsNullOrEmpty(value) )
			{
                textWriter.Write("0:");
			}
            else
            {
                textWriter.Write(value.Length + ":" + value);
            }
        }

        public void WriteNumber(long value)
        {
            textWriter.Write("i" + value + "e");
        }

        public void WriteList(IList value)
        {
            textWriter.Write("l");

            if (value != null)
            {
                foreach (var item in value)
                {
                    if (item != null)
                    {
                        WriteObject(item);
                    }
                }
            }

            textWriter.Write("e");
        }
        public void WriteDictionary(IDictionary value)
        {
            textWriter.Write("d");

            if (value != null)
            {
                foreach (var key in value.Keys)
                {
                    if (key != null && value[key] != null)
                    {
                        WriteObject(key);
                         
                        WriteObject(value[key] );
                    }
                }
            }

            textWriter.Write("e");
        }

        public void Dispose()
        {
            textWriter.Dispose();
        }
    }
}