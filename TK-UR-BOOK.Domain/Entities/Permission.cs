using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Domain.Entities
{
    public class Permission : BaseEntity<PermissionId>
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        private Permission()
        {
        }
        public Permission(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}
