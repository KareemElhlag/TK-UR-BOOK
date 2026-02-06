using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Domain.Entities
{
    public class RefreshToken : AuditTableEntity<long>
    {
        public UserId UserId { get;private set; }
        public string Token { get; private set; } = string.Empty;

        public DateTime ExpiresAt { get; private set; }
       public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public DateTime? RevokedAt { get; private set; }
        public string? RevokReason { get; private set; } = string.Empty;
        public bool IsRevoked => RevokedAt != default;

        public bool IsActive => !IsRevoked && !IsExpired;



        private RefreshToken() { }
        private RefreshToken(UserId userId, string token, DateTime expiresAt)
        {
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
        }

        public static RefreshToken CreateNew(UserId userId, string token, DateTime expiresAt)
        {
            if (expiresAt <= DateTime.UtcNow)
            {
                throw new ArgumentException("Expiration time must be in the future.", nameof(expiresAt));
            }
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));
            }
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "UserId cannot be null.");
            }

            return new RefreshToken(userId, token, expiresAt);
        }


        public void Revoke(string reason)
        {
            if (IsRevoked)
            {
                throw new InvalidOperationException("Token is already revoked.");
            }
            RevokedAt = DateTime.UtcNow;
            RevokReason = reason;
        }
        public void ExtendExpiration(DateTime newExpiration)
        {
            if (newExpiration <= DateTime.UtcNow)
            {
                throw new ArgumentException("New expiration time must be in the future.", nameof(newExpiration));
            }
            if (IsRevoked)
            {
                throw new InvalidOperationException("Cannot extend expiration of a revoked token.");
            }
            ExpiresAt = newExpiration;
        }

    }


}
