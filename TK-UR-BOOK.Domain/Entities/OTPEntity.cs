

using TK_UR_BOOK.Domain.Comman;

namespace TK_UR_BOOK.Domain.Entities
{
    public class OTPEntity
    {
       public  UserId userId { get; private set; }
        public string code { get; private set; }
        public DateTime ExpireAt { get; private set; }
        public bool IsUsed { get; private set; }

        public bool IsValid => !this.IsUsed && this.ExpireAt >= DateTime.UtcNow;

        public void MarkAsUsed()
        {
            this.IsUsed = true;
        }
        private OTPEntity() { }
        public OTPEntity(UserId userId, string code)
        {
            this.userId = userId;
            this.code = code;

            this.ExpireAt = DateTime.UtcNow.AddMinutes(5);
            this.IsUsed = false;
        }
    }
}
