namespace TK_UR_BOOK.Domain.Comman
{
    public abstract record  StrongTypedID
    {
        public Guid Value { get; init; }
        protected StrongTypedID(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("ID value cannot be empty.", nameof(value));
            }
            Value = value;
        }

        public override string ToString() => Value.ToString();
    }
}
