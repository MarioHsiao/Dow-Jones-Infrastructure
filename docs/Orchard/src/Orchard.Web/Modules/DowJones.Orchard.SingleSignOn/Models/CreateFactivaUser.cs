using Orchard.Security;

namespace DowJones.Orchard.SingleSignOn
{
    internal class CreateFactivaUser : CreateUserParams
    {
        public CreateFactivaUser(FactivaUserId userId)
            : base(userId, userId, userId, userId, userId, true)
        {
        }
    }
}