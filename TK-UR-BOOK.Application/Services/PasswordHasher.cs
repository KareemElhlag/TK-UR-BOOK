using TK_UR_BOOK.Application.Interfaces;
using BCrypt.Net;

namespace TK_UR_BOOK.Application.Services
{
    public class PasswordHasher : IHashingPassword
    {
        public string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        => BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }
}
