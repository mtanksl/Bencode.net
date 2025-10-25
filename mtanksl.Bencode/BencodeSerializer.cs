using mtanksl.Bencode.Linq;
using System;
using System.Linq;
using System.Reflection;

namespace mtanksl.Bencode
{
    public class BencodeSerializer
    {
        public BencodeSerializer() : this(null)
        {            
        }

        public BencodeSerializer(BencodeSerializerSettings settings)
        {
            this.settings = settings ?? new BencodeSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
        }

        private BencodeSerializerSettings settings;

        public BencodeSerializerSettings Settings
        {
            get
            {
                return settings;
            }
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// 
        public void Serialize(BencodeWriter writer, object value)
        {
            if (value == null)
			{
                throw new ArgumentNullException(nameof(value) );
            }

            Serialize(writer, value, value.GetType() );
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// 
        public void Serialize(BencodeWriter writer, object value, Type type)
        {
            if (writer == null)
			{
                throw new ArgumentNullException(nameof(writer) );
            }

            if (type == null)
			{
                throw new ArgumentNullException(nameof(type) );
            }

            var previous = writer.WriteObjectHandler;

            try
            {
                writer.WriteObjectHandler = (v, t) =>
                {
                    if (typeof(IBencodeSerializable).IsAssignableFrom(t) )
                    {
                        ( (IBencodeSerializable)v).Write(writer, t, this);

                        return true;
                    }

                    if (settings.Converters != null)
                    {
                        foreach (var converter in settings.Converters)
                        {
                            if (converter.CanConvert(t) )
                            {
                                converter.Write(writer, v, t, this);

                                return true;
                            }
                        }
                    }

                    var bencodeObjectAttribute = t.GetCustomAttribute<BencodeObjectAttribute>();

                    if (bencodeObjectAttribute != null)
                    {
                        if (bencodeObjectAttribute.ItemConverterType != null)
                        {
                            var converter = (BencodeConverter)Activator.CreateInstance(bencodeObjectAttribute.ItemConverterType, bencodeObjectAttribute.ItemConverterParameters);

                            converter.Write(writer, v, t, this);

                            return true;
                        }

                        writer.WriteByte( (byte)'d');

                        var properties = t.GetProperties()
                            .Select(p => new { PropertyInfo = p, BencodePropertyAttribute = p.GetCustomAttribute<BencodePropertyAttribute>() } )
                            .Where(a => a.BencodePropertyAttribute != null)
                            .OrderBy(a => a.BencodePropertyAttribute.PropertyName ?? a.PropertyInfo.Name);

                        foreach (var property in properties)
                        {                            
                            var propertyValue = property.PropertyInfo.GetValue(v);

                            if (propertyValue != null || (propertyValue == null && settings.NullValueHandling == NullValueHandling.Include) )
                            {
                                writer.WriteObject(property.BencodePropertyAttribute.PropertyName ?? property.PropertyInfo.Name, typeof(string) );

                                if (property.BencodePropertyAttribute.ItemConverterType != null)
                                {
                                    var converter = (BencodeConverter)Activator.CreateInstance(property.BencodePropertyAttribute.ItemConverterType, property.BencodePropertyAttribute.ItemConverterParameters);

                                    converter.Write(writer, propertyValue, property.PropertyInfo.PropertyType, this);
                                }
                                else
                                {
                                    writer.WriteObject(propertyValue, property.PropertyInfo.PropertyType);
                                }
                            }
                        }

                        writer.WriteByte( (byte)'e');

                        return true;
                    }

                    return false;
                };

                writer.WriteObject(value, type);
            }
            finally
            {
                writer.WriteObjectHandler = previous;
            }
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public object Deserialize(BencodeReader reader)
        {
            return Deserialize(reader, typeof(BElement) );
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public T Deserialize<T>(BencodeReader reader)
        {
            return (T)Deserialize(reader, typeof(T) );
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="BencodeException"></exception>
        /// 
        public object Deserialize(BencodeReader reader, Type type)
        {
            if (reader == null)
			{
                throw new ArgumentNullException(nameof(reader) );
            }

            if (type == null)
			{
                throw new ArgumentNullException(nameof(type) );
            }

            var previous = reader.ReadObjectHandler;

            try
            {
                reader.ReadObjectHandler = t =>
                {
                    if (typeof(IBencodeSerializable).IsAssignableFrom(t) )
                    {
                        var value = (IBencodeSerializable)Activator.CreateInstance(t);

                        value.Read(reader, t, this);

                        return value;
                    }

                    if (settings.Converters != null)
                    {
                        foreach (var converter in settings.Converters)
                        {
                            if (converter.CanConvert(t) )
                            {
                                return converter.Read(reader, t, this);
                            }
                        }
                    }

                    var bencodeObjectAttribute = t.GetCustomAttribute<BencodeObjectAttribute>();

                    if (bencodeObjectAttribute != null)
                    {
                        if (bencodeObjectAttribute.ItemConverterType != null)
                        {
                            var converter = (BencodeConverter)Activator.CreateInstance(bencodeObjectAttribute.ItemConverterType, bencodeObjectAttribute.ItemConverterParameters);

                            return converter.Read(reader, t, this);
                        }

                        if ( (char)reader.ReadByte() == 'd')
                        {
                            var value = Activator.CreateInstance(t);

                            while ( (char)reader.PeekByte() != 'e')
                            {
                                var propertyName = (string)reader.ReadObject(typeof(string) );

                                var property = t.GetProperties()
                                    .Select(p => new { PropertyInfo = p, BencodePropertyAttribute = p.GetCustomAttribute<BencodePropertyAttribute>() } )
                                    .Where(a => a.BencodePropertyAttribute != null &&
                                                (a.BencodePropertyAttribute.PropertyName ?? a.PropertyInfo.Name) == propertyName)
                                    .FirstOrDefault();
                                
                                if (property != null)
                                {
                                    if (property.BencodePropertyAttribute.ItemConverterType != null)
                                    {
                                        var converter = (BencodeConverter)Activator.CreateInstance(property.BencodePropertyAttribute.ItemConverterType, property.BencodePropertyAttribute.ItemConverterParameters);

                                        property.PropertyInfo.SetValue(value, converter.Read(reader, property.PropertyInfo.PropertyType, this) );
                                    }
                                    else
                                    {
                                        property.PropertyInfo.SetValue(value, reader.ReadObject(property.PropertyInfo.PropertyType) );
                                    }
                                }
                                else
                                {
                                    reader.ReadObject();
                                }
                            }

                            reader.ReadByte();

                            return value;
                        }
                    }

                    return null;
                };
                
                return reader.ReadObject(type);
            }
            finally
            {
                reader.ReadObjectHandler = previous;
            }
        }
    }
}