namespace TK_UR_BOOK.Application.UseCases.UserCommands
{
    public record ConfirmEmailCommand(
        string Email,
        string Token
    );


}
