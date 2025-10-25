using System;

namespace mtanksl.Bencode.Converters
{
    public class DateTimeConverter : BencodeConverter
    {
        public override bool CanConvert(Type type)
        {
            return type == typeof(DateTime) || type == typeof(DateTime?);
        }

        public override object Read(BencodeReader reader, Type type, BencodeSerializer serializer)
        {
            if (type == typeof(DateTime) )
            {
                var value = reader.ReadInteger();

                if (value != null)
                {
                    return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(value.Value);
                }

                return DateTime.MinValue;
            }
            else if (type == typeof(DateTime?) )
            {
                var value = reader.ReadInteger();

                if (value != null)
                {
                    return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(value.Value);
                }

                return null;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override void Write(BencodeWriter writer, object value, Type type, BencodeSerializer serializer)
        {
            if (type == typeof(DateTime) )
            {
                writer.WriteInteger( (long)( (DateTime)value - new DateTime(1970, 1, 1, 0, 0, 0) ).TotalSeconds);
            }
            else if (type == typeof(DateTime?) )
            {
                if (value != null)
                {
                    writer.WriteInteger( (long)( (DateTime)value - new DateTime(1970, 1, 1, 0, 0, 0) ).TotalSeconds);
                }
                else
                {
                    writer.WriteInteger(null);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}