using System;
using System.Collections.Generic;

namespace mtanksl.Bencode.Linq
{
    public class BList : BElement
    {
        public BList(params BElement[] values) : this( (IList<BElement>)values)
        { 
        }

        public BList(IList<BElement> value)
        {
            Value = new List<BElement>(value);
        }

        public List<BElement> Value { get; }

        public BElement this[int index]
        {
            get
            {
                return Value[index];
            }
            set
            {
                Value[index] = value;
            }
        }

        /// <exception cref="FormatException"></exception>
        /// 
        public override BElement this[object key]
        {
            get
            {
                if (key is int)
                {
                    return this[ (int)key ];
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

        //TODO: Implement IList interface
    }
}