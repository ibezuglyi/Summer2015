using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using WebApp.Models;
using WebApp.Models.Account;
using System.Security.Cryptography;
using System.Text;
using WebApp.Services;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    public class CandidateController : UserController
    {
        private IApplicationService service;

        public CandidateController(IApplicationService applicationService)
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
            var user = await service.GetCandidateByEmailAsync(model.Email);
            if (user == null)
            {
                WrongEmailPasswordError();
                return View(model);
            }

            var hashPassword = service.GenerateHashPassword(model.Password, user);
            if (hashPassword == user.Password)
            {
                var identity = CreateIdentity(user, "Candidate");
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

            var user = await service.GetCandidateByEmailAsync(model.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "User with this email already exists.");
                return View(model);
            }
            await service.CreateCandidateUserAsync(model);
            return RedirectToAction("Index", "Home");
        }

    }
}