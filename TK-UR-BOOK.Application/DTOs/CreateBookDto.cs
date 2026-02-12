using TK_UR_BOOK.Domain.Enums;

namespace TK_UR_BOOK.Application.DTOs
{
    public class CreateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; }
        public string ISBN { get; set; }
        public int CategoryId { get; set; }
        public decimal PriceAmount { get; set; }
        public Currency Currency { get; set; }
        public int Stock { get; set; }


    }
}
