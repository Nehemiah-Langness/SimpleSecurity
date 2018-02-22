namespace Security.Common.Bases
{
    public abstract class PassThrough<T>
    {
        protected readonly T Value;

        protected PassThrough(T value)
        {
            Value = value;
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override bool Equals(object obj) => Value.Equals(obj);
        public override string ToString() => Value.ToString();
    }
}
