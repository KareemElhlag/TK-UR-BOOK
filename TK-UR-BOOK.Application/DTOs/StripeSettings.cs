namespace TK_UR_BOOK.Application.DTOs
{
    public class StripeSettings
    {
        public string secretKey { get; set; } = string.Empty;
        public string publishsbleKey { get; set; } = string.Empty;
        public string webhookKey { get; set; } = string.Empty;
    }
}
