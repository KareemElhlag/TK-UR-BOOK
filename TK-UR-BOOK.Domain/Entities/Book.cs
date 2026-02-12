using TK_UR_BOOK.Domain.Comman;
using TK_UR_BOOK.Domain.ValueObjects;

namespace TK_UR_BOOK.Domain.Entities
{
    public class Book : AuditTableEntity<BookId>
    {
        public string Title { get; private set; }
        public string Author { get; private set; }
        /// <summary>
        /// ISBN (International Standard Book Number) is a unique identifier for books, allowing for efficient cataloging and tracking in the publishing industry. 
        ///It typically consists of 13 digits and helps distinguish one book from another, 
        ///even if they have similar titles or authors.
        /// </summary>
        public string ISBN { get; private set; }
        public Money Price { get; private set; }
        public string Description { get; private set; }
        public int StockQuantity { get; private set; }
        public int CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;
        public bool IsDeleted { get; private set; }
        private Book()
        {
        }
        public Book(BookId bookId, string title, string author, string isbn, Money price, int Qun, int categoryId,
        string description = "")
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty", nameof(title));
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Author cannot be null or empty", nameof(author));
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("ISBN cannot be null or empty", nameof(isbn));
            if (Qun < 0)
                throw new ArgumentException("Stock quantity cannot be negative", nameof(Qun));
            if (price.Amount < 0)
                throw new ArgumentException("Price cannot be negative", nameof(price));
            Id = bookId;

            Title = title;
            Author = author;
            ISBN = isbn;
            Price = price;
            StockQuantity = Qun;
            CategoryId = categoryId;
            Description = description;
        }

        public void UpdatePrice(Money newPrice)
        {
            if (newPrice.Amount < 0)
                throw new ArgumentException("Price cannot be negative", nameof(newPrice));
            Price = newPrice;
        }

        public void AdjustStock(int quantityChange)
        {
            int newQuantity = StockQuantity + quantityChange;
            if (newQuantity < 0)
                throw new InvalidOperationException("Stock quantity cannot be negative");
            StockQuantity = newQuantity;
        }
        public void UpdateDetails(string title, string author, string description)
        {
            Title = title;
            Author = author;
            Description = description;
        }
        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }
    }
}
