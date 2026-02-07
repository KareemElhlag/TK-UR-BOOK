using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Domain.Entities
{
    public class Group : BaseEntity<GroupId>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        private readonly List<Permission> _permissions = new();        
        public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();
        private readonly List<User> _users = new();

        public IReadOnlyCollection<User> Users => _users.AsReadOnly();
        private Group()
        {
        }
        public Group(GroupId id ,string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
        public void AddPermission(Permission permission)
        {
            if (!_permissions.Any(p => p.Id == permission.Id))
            {
                _permissions.Add(permission);
            }
        }

        public void RemovePermission(PermissionId permissionId)
        {
            var permission = _permissions.FirstOrDefault(p => p.Id == permissionId);
            if (permission != null)
            {
                _permissions.Remove(permission);
            }
        }
    }
}
