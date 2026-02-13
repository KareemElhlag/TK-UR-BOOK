using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Domain.Entities
{
    public class User : AuditTableEntity<UserId>
    {
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? EmailVerificationToken { get; private set; }
        public bool IsEmailConfirmed { get; private set; }
        public bool IsActive { get; private set; } = true;
        public int AccessFailedCount { get; private set; }
        public DateTimeOffset? LockoutEnd { get; private set; }

        private readonly List<Group> _groups = new();
        public IReadOnlyCollection<Group> Groups => _groups.AsReadOnly();

        private User()
        {
        }
        public User(UserId userId, string username, string email, string passwordHash, string? phoneNumber = null)
        {
            Id = userId;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            PhoneNumber = phoneNumber;
            IsEmailConfirmed = false;
        }


        public void AddToGroup(Group group)
        {
            if (!_groups.Any(g => g.Id == group.Id))
            {
                _groups.Add(group);
            }
        }

        public void UpdatePassword(string newHash)
        {
            PasswordHash = newHash;
        }
        public void Deactivate() => IsActive = false;

        public void GenerateEmailVerificationToken()
        {
            EmailVerificationToken = Guid.NewGuid().ToString();
        }

        public void ConfirmEmail()
        {
            IsEmailConfirmed = true;
            EmailVerificationToken = null;
        }

    }
}
