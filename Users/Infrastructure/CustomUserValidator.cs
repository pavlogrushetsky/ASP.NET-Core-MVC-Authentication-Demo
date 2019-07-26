using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Users.Models;

namespace Users.Infrastructure
{
    public class CustomUserValidator : UserValidator<AppUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var identityResult = await base.ValidateAsync(manager, user);

            var errors = identityResult.Succeeded 
                ? new List<IdentityError>() 
                : identityResult.Errors.ToList();

            if (!user.Email.ToLower().EndsWith("@example.com"))
            {
                errors.Add(new IdentityError
                {
                    Code = "EmailDomainError",
                    Description = "Only example.com email address are allowed"
                });
            }

            return errors.Count == 0
                ? IdentityResult.Success
                : IdentityResult.Failed(errors.ToArray());
        }
    }
}
