using mtanksl.Bencode.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mtanksl.Bencode
{
    public class BencodeWriter : IDisposable
    {
        private Stream stream;

        private HashSet<object> references = new HashSet<object>();

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// 
        public BencodeWriter(Stream stream)
        {
            if (stream == null)
			{
				throw new ArgumentNullException(nameof(stream) );
			}

            if ( !stream.CanWrite)
            {
				throw new NotSupportedException();
            }

            this.stream = stream;
        }

        public void WriteByte(byte value)
        {
            stream.WriteByte(value);
        }

        internal Func<object, Type, bool> WriteObjectHandler { get; set; }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public void WriteObject(object value, Type type)
        {
            if (type == null)
			{
                throw new ArgumentNullException(nameof(type) );
            }
            
            if (value != null)
            {
                type = value.GetType();
            }
            
            if (type == typeof(string) )
            {
                WriteString( (string)value);
            }
            else if (type == typeof(byte[] ) )
            {
                WriteByteString( (byte[] )value);
            }
            else if (type == typeof(BString) )
            {
                if (value != null)
                {
                    WriteByteString( ( (BString)value).Value);
                }
                else
                {
                    WriteByteString(null);
                }
            }
            else if (type == typeof(sbyte) || type == typeof(byte) || type == typeof(short) || type == typeof(ushort) || type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) )
            {
                WriteInteger( (long)value);
            }
            else if (type == typeof(sbyte?) || type == typeof(byte?) || type == typeof(short?) || type == typeof(ushort?) || type == typeof(int?) || type == typeof(uint?) || type == typeof(long?) || type == typeof(ulong?) )
            {
                WriteInteger( (long?)value);
            }
            else if (type == typeof(BNumber) )
            {
                if (value != null)
                {
                    WriteInteger( ( (BNumber)value).Value);
                }
                else
                {
                    WriteInteger(null);
                }
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) )
            {
                var genericArguments = type.GetGenericArguments();

                WriteList( (IList)value, genericArguments[0] );
            }
            else if (type == typeof(BList) )
            {
                if (value != null)
                {
                    WriteList( ( (BList)value).Value, typeof(BElement) );
                }
                else
                {
                    WriteList(null, typeof(BElement) );
                }
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SortedDictionary<,>) )
            {
                var genericArguments = type.GetGenericArguments();

                WriteDictionary( (IDictionary)value, genericArguments[0], genericArguments[1] );
            }
            else if (type == typeof(BDictionary) )
            {
                if (value != null)
                {
                    WriteDictionary( ( (BDictionary)value).Value, typeof(BString), typeof(BElement) );
                }
                else
                {
                    WriteDictionary(null, typeof(BString), typeof(BElement) );
                }
            }
            else if (WriteObjectHandler == null || !WriteObjectHandler(value, type) )
            {
                throw new NotSupportedException();
            }
        }

        public void WriteString(string value)
        {
            if (string.IsNullOrEmpty(value) )
            {
                WriteByte( (byte)'0');

                WriteByte( (byte)':');
            }
            else
            {
                WriteByteString(Encoding.UTF8.GetBytes(value) );
            }
        }

        public void WriteByteString(byte[] value)
        {
            if (value == null)
            {
                WriteByte( (byte)'0');

                WriteByte( (byte)':');
            }
            else
            {
                foreach (var item in value.Length.ToString() )
                {
                    WriteByte( (byte)item);
                }

                WriteByte( (byte)':');

                foreach (var item in value)
                {
                    WriteByte(item);
                }
            }
        }

        public void WriteInteger(long? value)
        {
            WriteByte( (byte)'i');

            if (value != null)
            {
                foreach (var item in value.ToString() )
                {
                    WriteByte( (byte)item);
                }
            }

            WriteByte( (byte)'e');
        }

        /// <exception cref="BencodeException"></exception>
        /// 
        public void WriteList(IList value, Type type)
        {               
            if (type == null)
			{
                throw new ArgumentNullException(nameof(type) );
            }

            WriteByte( (byte)'l');

            if (value != null)
            {
                if (references.Contains(value) )
                {
                    throw new BencodeException("Loop encountered.");
                }

                references.Add(value);

                foreach (var item in value)
                {
                    WriteObject(item, type);
                }
            }
                
            WriteByte( (byte)'e');
        }

        /// <exception cref="BencodeException"></exception>
        /// 
        public void WriteDictionary(IDictionary value, Type typeKey, Type typeValue)
        {
            if (typeKey == null)
			{
                throw new ArgumentNullException(nameof(typeKey) );
            }

            if (typeValue == null)
			{
                throw new ArgumentNullException(nameof(typeValue) );
            }

            WriteByte( (byte)'d');

            if (value != null)
            {
                if (references.Contains(value) )
                {
                    throw new BencodeException("Loop encountered.");
                }

                references.Add(value);
                                
                foreach (var key in value.Keys)
                {
                    WriteObject(key, typeKey);
                         
                    WriteObject(value[key], typeValue);
                }
            }

            WriteByte( (byte)'e');
        }

        public void Dispose()
        {
            stream.Dispose();
        }
    }
}