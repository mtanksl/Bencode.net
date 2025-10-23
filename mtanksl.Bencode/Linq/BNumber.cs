namespace mtanksl.Bencode.Linq
{
    public class BNumber : BElement
    {
        public BNumber(BNumber value) : this(value.Value)
        {            
        }

        public BNumber(long value)
        {
            Value = value;
        }

        public long Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator BNumber(long value)
        {
            return new BNumber(value);
        }

        public static explicit operator long(BNumber value)
        {
            return value.Value;
        }
    }
}