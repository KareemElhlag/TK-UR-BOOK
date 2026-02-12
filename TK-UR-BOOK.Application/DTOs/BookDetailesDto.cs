namespace TK_UR_BOOK.Application.DTOs
{
    public class BookDetailesDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public decimal Price { get; set; } // الـ Amount [cite: 2026-02-04]
        public string Currency { get; set; } = string.Empty; // الـ Enum Name [cite: 2026-02-04]
        public int Stock { get; set; }
        public string? CategoryName { get; internal set; }
    }
}
