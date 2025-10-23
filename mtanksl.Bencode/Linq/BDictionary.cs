using System;
using System.Collections.Generic;
using System.Linq;

namespace mtanksl.Bencode.Linq
{
    public class BDictionary : BElement
    {
        public BDictionary(params BPair[] values) : this(values.ToDictionary(v => v.Key, v => v.Value) ) 
        { 
        }

        public BDictionary(IDictionary<BString, BElement> value)
        {
            Value = new SortedDictionary<BString, BElement>(value);
        }

        public SortedDictionary<BString, BElement> Value { get; }

        public BElement this[BString key]
        {
            get
            {
                if (Value.TryGetValue(key, out var value) )
                {
                    return value;
                }

                return null;
            }
        }

        /// <exception cref="FormatException"></exception>
        /// 
        public override BElement this[object key]
        {
            get
            {
                if (key is string)
                {
                    return this[ (string)key ];
                }
                else if (key is BString)
                {
                    return this[ (BString)key ];
                }

                throw new FormatException();
            }
        }

        public int Count
        {
            get
            {
                return Value.Count;
            }
        }

        //TODO: Implement IDicionary interface
    }

    public class BPair
    {
        public BPair(BString key, BElement value)
        {
            Key = key;

            Value = value;
        }

        public BString Key { get; set; }

        public BElement Value { get; set; }
    }
}