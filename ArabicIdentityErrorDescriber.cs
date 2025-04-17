using Microsoft.AspNetCore.Identity;

namespace KauRestaurant.Services
{
    public class ArabicIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
            => new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = $"البريد الإلكتروني '{userName}' مستخدم مسبقاً."
            };
    }
}
