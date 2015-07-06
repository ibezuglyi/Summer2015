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
    public class RecruiterController : UserController
    {
        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model )
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var blogContext = new JobContext();
            var user = await blogContext.RecruiterUsers.Find(x => x.Email == model.Email).SingleOrDefaultAsync();
            if (user == null)
            {
                WrongEmailPasswordError();
                return View(model);
            }

            var hashPassword = GenerateHashPassword(model.Password, user);
            if (hashPassword == user.Password)
            {
                var identity = CreateIdentity(user);
                SignIn(identity);

                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            WrongEmailPasswordError();
            return View(model);           
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var blogContext = new JobContext();
            var userFound = await blogContext.RecruiterUsers.Find(x => x.Email == model.Email).FirstOrDefaultAsync();
            if (userFound != null)
            {
                DuplicateEmailError();
                return View(model);
            }
            await CreateRecruiterUser(model, blogContext);
            return RedirectToAction("Index", "Home");
        }

        public async Task CreateRecruiterUser(RegisterModel model, JobContext jobContext)
        {
            var user = new RecruiterUser
            {
                Name = model.Name,
                Email = model.Email,
            };

            user.Password = GenerateHashPassword(model.Password, user);

            await jobContext.RecruiterUsers.InsertOneAsync(user);
        }
  
    }
}