﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Models.Account;
using WebApp.Services;
using System.Web.Script.Serialization;
using WebApp.Models;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    public class CandidateController : Controller
    {
        
        private IApplicationService service;

        public CandidateController(IApplicationService applicationService)
        {
            service = applicationService;
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
            var user = await service.GetCandidateByEmailAsync(model.Email);
            if (user == null)
            {
                AddWrongEmailPasswordError();
                return View(model);
            }

            var hashPassword = service.GenerateHashPassword(model.Password, user);
            if (hashPassword == user.Password)
            {
                var identity = service.CreateCandidateIdentity(user);
                service.SignIn(identity, Request);

                return Redirect(GetRedirectUrl(model.ReturnUrl));
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

            var user = await service.GetCandidateByEmailAsync(model.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "User with this email already exists.");
                return View(model);
            }
            await service.CreateCandidateUserAsync(model);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            service.SignOut(Request);
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

        [HttpGet]
        public JsonResult GetHints(string query)
        {
            List<string> hints = new List<string>();
            hints.Add("C#");
            hints.Add("Html");

            var response = new SkillHintsModel(query, hints);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}