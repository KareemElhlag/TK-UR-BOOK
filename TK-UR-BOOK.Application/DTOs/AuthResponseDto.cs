namespace TK_UR_BOOK.Application.DTOs
{
    public class AuthResponseDto
    {
        public Guid? UserId { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public  string? expiredToken { get; set; }
       public string? refreshToken { get; set; }
    }
}
