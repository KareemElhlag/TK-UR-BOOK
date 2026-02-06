namespace TK_UR_BOOK.Domain.Comman
{
    public sealed record UserId (Guid value) : StrongTypedID(value)
    {
        public static UserId NewId() => new UserId(Guid.NewGuid());
    };
    public sealed record BookId(Guid Value) : StrongTypedID(Value)
    {
        public static BookId NewId() => new BookId(Guid.NewGuid());
    };
    public record GroupId (int Value);
    public record PermissionId(int Value);

}
