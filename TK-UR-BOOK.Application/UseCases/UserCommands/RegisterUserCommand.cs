using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Application.UseCases.UserCommands
{
    public record RegisterUserCommand(
        string Name,
        string Email,
        string Password,
        string ? PhoneNumber,
        GroupId? GroupId = null
    );



}
