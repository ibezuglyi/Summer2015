using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Models.Account;
using WebApp.Services;
using System.Web.Script.Serialization;
using WebApp.Models;
using WebApp.Entities;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    public class CandidateController : Controller
    {
        
        private IApplicationService _applicationService;
        private IAuthenticationService _authenticationService;

        public CandidateController(IApplicationService applicationService, IAuthenticationService authenticationService)
        {
            _applicationService = applicationService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _applicationService.GetCandidateByEmailAsync(model.Email);
            if (user == null)
            {
                AddWrongEmailPasswordError();
                return View(model);
            }

            var hashPassword = _applicationService.GenerateHashPassword(model.Password, user);
            if (hashPassword == user.Password)
            {
                var identity = _authenticationService.CreateCandidateIdentity(user);
                _authenticationService.SignIn(identity, Request);
                if (user.HasSkills())
                {
                    return Redirect(GetRedirectUrl(model.ReturnUrl));                    
                }
                return RedirectToAction("Index", "CandidateProfile");
            }
            AddWrongEmailPasswordError();
            return View(model);
        }        

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _applicationService.GetCandidateByEmailAsync(model.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "User with this email already exists.");
                return View(model);
            }
            await _applicationService.CreateCandidateUserAsync(model);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            _authenticationService.SignOut(Request);
            return RedirectToAction("Index", "Home");
        }

        public string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }

        public void AddWrongEmailPasswordError()
        {
            ModelState.AddModelError("Email", "Wrong email address or password.");
        }

        public void AddDuplicatedEmailError()
        {
            ModelState.AddModelError("Email", "User with this email already exists.");
        }

    }
}