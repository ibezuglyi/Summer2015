﻿using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using WebApp.Models;
using WebApp.Models.Account;
using System.Security.Cryptography;
using System.Text;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterModel());
        }

        public string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }

        [HttpPost]
        public ActionResult Logout()
        {
            var authManager = GetAuthManager();
            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }

        public void SignIn(ClaimsIdentity identity)
        {
            var authManager = GetAuthManager();
            authManager.SignIn(identity);
        }

        public static ClaimsIdentity CreateIdentity(User user)
        {
            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                }, "ApplicationCookie");
            return identity;
        }

        public Microsoft.Owin.Security.IAuthenticationManager GetAuthManager()
        {
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;
            return authManager;
        }

        public void WrongEmailPasswordError()
        {
            ModelState.AddModelError("Email", "Wrong email address or password.");
        }

        public void DuplicateEmailError()
        {
            ModelState.AddModelError("Email", "User with this email already exists.");
        }

        public string GenerateHashPassword(string password, User user)
        {
            SHA1 sha1 = SHA1.Create();
            string dataToHash = user.Name + password + user.Email;
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(dataToHash));
            StringBuilder returnValue = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            return returnValue.ToString();
        }  
    }
}