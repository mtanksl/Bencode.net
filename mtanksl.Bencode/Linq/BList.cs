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
                if (0 <= index && index < Value.Count)
                {
                    return Value[index];
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

        public bool Contains(BElement value)
        {
            return Value.Contains(value);
        }

        public void Add(BElement value)
        {
            Value.Add(value);
        }

        public void Remove(BElement value)
        {
            Value.Remove(value);
        }

        public void Insert(int index, BElement value)
        {
            Value.Insert(index, value);
        }

        public void RemoveAt(int index)
        {
            Value.RemoveAt(index);
        }

        public void Clear()
        {
            Value.Clear();
        }

        //TODO: Implement IList interface
    }
}