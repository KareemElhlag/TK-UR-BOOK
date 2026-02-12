namespace TK_UR_BOOK.Application.DTOs
{
    public class SpecParams
    {
        public int pageindex { get; set; }
        public int pageSize { get; set; }
        public string? search { get; set; }
        public int? price { get; set; }

        public string? sort { get; set; }
        public decimal? Maxprice { get; set; }
        public decimal? Minprice { get; set; }
    }
}
