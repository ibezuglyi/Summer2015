using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using WebApp.Models;
using WebApp.Models.Account;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    public class CandidateController : UserController
    {

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var blogContext = new BlogContext();
            var user = await blogContext.CandidateUsers.Find(x => x.Email == model.Email).SingleOrDefaultAsync();
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email address has not been registered.");
                return View(model);
            }
            var identity = CreateIdentity(user);
            SignIn(identity);

            return Redirect(GetRedirectUrl(model.ReturnUrl));
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var blogContext = new BlogContext();
            var userFound = await blogContext.CandidateUsers.Find(x => x.Email == model.Email).FirstOrDefaultAsync();
            if (userFound != null)
            {
                ModelState.AddModelError("Email", "User with this email already exists.");
                return View(model);
            }
            await CreateCandidateUser(model, blogContext);
            return RedirectToAction("Index", "Home");
        }

        public static async Task CreateCandidateUser(RegisterModel model, BlogContext blogContext)
        {
            var user = new CandidateUser
            {
                Name = model.Name,
                Email = model.Email
            };

            await blogContext.CandidateUsers.InsertOneAsync(user);
        }
    }
}