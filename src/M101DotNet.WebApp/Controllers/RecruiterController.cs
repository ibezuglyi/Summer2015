﻿using System.Runtime.Remoting.Messaging;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using MongoDB.Driver;
using WebApp.Models;
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
                WrongEmailPasswordError();
                return View(model);
            }

            var hashPassword = service.GenerateHashPassword(model.Password, user);
            if (hashPassword == user.Password)
            {
                var identity = CreateIdentity(user, "Recruiter");
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

            var user = await service.GetRecruterByEmailAsync(model.Email);
            if (user != null)
            {
                DuplicateEmailError();
                return View(model);
            }
            await service.CreateRecruiterUserAsync(model);
            return RedirectToAction("Index", "Home");
        }
    }
}