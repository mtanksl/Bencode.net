using System;
using System.Collections.Generic;
using System.IO;

namespace mtanksl.Bencode
{
    public class BencodeWriter : IDisposable
    {
        private TextWriter textWriter;

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

            if (value is string)
            {
                WriteString( (string)value);
            }
            else if (value is sbyte || value is byte || value is short || value is ushort || value is int || value is uint || value is long || value is ulong)
            {
                WriteInteger( (long)value);
            }
            else if (value is List<object>)
            {
                WriteList( (List<object>)value);
            }
            else if (value is Dictionary<object, object>)
            {
                WriteDictionary( (Dictionary<object, object>)value);
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

            textWriter.Write(value.Length + ":" + value);
        }

        public void WriteInteger(long value)
        {
            textWriter.Write("i" + value + "e");
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// 
        public void WriteList(List<object> value)
        {
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value) );
			}

            textWriter.Write("l");

            foreach (var item in value)
            {
                WriteObject(item);
            }

            textWriter.Write("e");
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// 
        public void WriteDictionary(Dictionary<object, object> value)
        {
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value) );
			}

            textWriter.Write("d");

            foreach (var item in value)
            {
                WriteObject(item.Key);

                WriteObject(item.Value);
            }

            textWriter.Write("e");
        }

        public void Dispose()
        {
            textWriter.Dispose();
        }
    }
}