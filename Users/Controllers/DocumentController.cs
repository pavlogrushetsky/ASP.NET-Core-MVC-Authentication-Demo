using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Users.Models;

namespace Users.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly ProtectedDocument[] _documents = new ProtectedDocument[]
        {
            new ProtectedDocument
            {
                Title = "Q3 Budget",
                Author = "Alice",
                Editor = "Joe"
            },
            new ProtectedDocument
            {
                Title = "Project Plan",
                Author = "Bob",
                Editor = "Alice"
            }
        };
        private readonly IAuthorizationService _authorization;

        public DocumentController(IAuthorizationService authorization)
        {
            _authorization = authorization;
        }

        public ViewResult Index() => View(_documents);

        public async Task<IActionResult> Edit(string title)
        {
            var document = _documents.FirstOrDefault(d => d.Title == title);
            var authorized = await _authorization.AuthorizeAsync(User, document, "AuthorsAndEditors");

            if (authorized.Succeeded)
            {
                return View("Index", document);
            }
            else
            {
                return new ChallengeResult();
            }
        }
    }
}
