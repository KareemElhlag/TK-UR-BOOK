using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK_UR_BOOK.Domain.Enums;

namespace TK_UR_BOOK.Application.DTOs
{
    public class UpdateBookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal PriceAmount { get; set; } 
        public string Currency { get; set; }
        public int ChangeStock { get; set; }

    }
}
