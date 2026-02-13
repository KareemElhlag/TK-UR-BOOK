namespace TK_UR_BOOK.Application.Interfaces
{
    public interface IHashingPassword
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}
