using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Models.Account;
using WebApp.Services;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    public class RecruiterController : UserController
    {
        private IApplicationService service;

        public RecruiterController(IApplicationService applicationService)
        {
            service = applicationService;
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await service.GetRecruterByEmailAsync(model.Email);
            if (user == null)
            {
                AddWrongEmailPasswordError();
                return View(model);
            }

            var hashPassword = service.GenerateHashPassword(model.Password, user);
            if (hashPassword == user.Password)
            {
                var identity = CreateIdentity(user, "Recruiter");
                SignIn(identity);

                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            AddWrongEmailPasswordError();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await service.GetRecruterByEmailAsync(model.Email);
            if (user != null)
            {
                AddDuplicatedEmailError();
                return View(model);
            }
            await service.CreateRecruiterUserAsync(model);
            return RedirectToAction("Index", "Home");
        }
    }
}