using TK_UR_BOOK.Domain.Common;
using TK_UR_BOOK.Domain.Enums;

namespace TK_UR_BOOK.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Amount { get; private set; }
        public Currency Currency { get; private set; }
        private Money()
        {
        }
        public Money(decimal amount, Currency currency)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative", nameof(amount));

            Amount = amount;
            Currency = currency;
        }


        public override string ToString()
        {
            return $"{Amount} {Currency}";
        }

        public Money AddMoney(Money other)
        {
            if (other.Currency != this.Currency)
                throw new InvalidOperationException("Cannot add money with different currencies");
            return new Money(this.Amount + other.Amount, this.Currency);
        }

        public Money SubtractMoney(Money other)
        {
            if (other.Currency != this.Currency)
                throw new InvalidOperationException("Cannot subtract money with different currencies");
            if (this.Amount < other.Amount)
                throw new InvalidOperationException("Resulting amount cannot be negative");
            return new Money(this.Amount - other.Amount, this.Currency);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}
