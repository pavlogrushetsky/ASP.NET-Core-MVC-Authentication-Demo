using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Users.Models;

namespace Users.Infrastructure
{
    public class DocumentAuthorizationRequirement : IAuthorizationRequirement
    {
        public bool AllowAuthors { get; set; }

        public bool AllowEditors { get; set; }
    }

    public class DocumentAuthorizationHandler : AuthorizationHandler<DocumentAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            DocumentAuthorizationRequirement requirement)
        {
            var protectedDocument = context.Resource as ProtectedDocument;
            var user = context.User.Identity.Name;
            var compare = StringComparison.OrdinalIgnoreCase;

            if (protectedDocument != null && 
                user != null && 
                (requirement.AllowAuthors && protectedDocument.Author.Equals(user, compare) || 
                    requirement.AllowEditors && protectedDocument.Editor.Equals(user, compare)))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
