using mtanksl.Bencode.Linq;
using System;
using System.Linq;
using System.Reflection;

namespace mtanksl.Bencode
{
    public class BencodeSerializer
    {
        private BencodeSerializerSettings settings;

        public BencodeSerializer() : this(null)
        {            
        }

        public BencodeSerializer(BencodeSerializerSettings settings)
        {
            this.settings = settings ?? BencodeSerializerSettings.Default;
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
                        ( (IBencodeSerializable)v).Write(writer);

                        return true;
                    }

                    var objectAttribute = t.GetCustomAttribute<BencodeObjectAttribute>();

                    if (objectAttribute != null)
                    {
                        writer.WriteByte( (byte)'d');

                        var properties = t.GetProperties()
                            .Select(p => new { PropertyInfo = p, PropertyAttribute = p.GetCustomAttribute<BencodePropertyAttribute>() } )
                            .Where(a => a.PropertyAttribute != null)
                            .OrderBy(a => a.PropertyAttribute.PropertyName ?? a.PropertyInfo.Name);

                        foreach (var property in properties)
                        {                            
                            var propertyValue = property.PropertyInfo.GetValue(v);

                            if (propertyValue != null || (propertyValue == null && settings.NullValueHandling == NullValueHandling.Include) )
                            {
                                writer.WriteObject(property.PropertyAttribute.PropertyName ?? property.PropertyInfo.Name, typeof(string) );

                                writer.WriteObject(propertyValue, property.PropertyInfo.PropertyType);
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

                        value.Read(reader);

                        return value;
                    }

                    var objectAttribute = t.GetCustomAttribute<BencodeObjectAttribute>();

                    if (objectAttribute != null)
                    {
                        if ( (char)reader.ReadByte() == 'd')
                        {
                            var value = Activator.CreateInstance(t);

                            while ( (char)reader.PeekByte() != 'e')
                            {
                                var propertyName = (string)reader.ReadObject(typeof(string) );

                                PropertyInfo p = null;

                                foreach (var propertyInfo in t.GetProperties() )
                                {
                                    var propertyAttribute = propertyInfo.GetCustomAttribute<BencodePropertyAttribute>();

                                    if (propertyAttribute != null)
                                    {
                                        if (propertyName == (propertyAttribute.PropertyName ?? propertyInfo.Name) )
                                        {
                                            p = propertyInfo;

                                            break;
                                        }
                                    }
                                }

                                if (p != null)
                                {
                                    p.SetValue(value, reader.ReadObject(p.PropertyType) );
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