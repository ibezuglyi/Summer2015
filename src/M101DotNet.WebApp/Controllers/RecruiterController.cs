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
    public class RecruiterController : Controllers.UserController
    {
        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model )
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var blogContext = new BlogContext();
            var user = await blogContext.RecruiterUsers.Find(x => x.Email == model.Email).SingleOrDefaultAsync();
            if (user == null)
            {
                ModelState.AddModelError("Email", "Wrong email address and password.");
                return View(model);
            }

            var hashPassword = GenerateHashPassword(model.Password, user);
            if (hashPassword == user.Password)
            {
                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                }, "ApplicationCookie");

                var context = Request.GetOwinContext();
                var authManager = context.Authentication;
                authManager.SignIn(identity);

            return Redirect(GetRedirectUrl(model.ReturnUrl));
        }

            ModelState.AddModelError("Email", "Wrong email address and password.");
            return View(model);           
        }



        [HttpPost]
        public ActionResult Logout()
        {
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
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

            var blogContext = new BlogContext();
            var userFound = await blogContext.RecruiterUsers.Find(x => x.Email == model.Email).FirstOrDefaultAsync();
            if (userFound != null)
            {
                ModelState.AddModelError("Email", "User with this email already exists.");
                return View(model);
            }
            await CreateRecruiterUser(model, blogContext);
            return RedirectToAction("Index", "Home");
        }


        public static async Task CreateRecruiterUser(RegisterModel model, BlogContext blogContext)
        {
            var user = new RecruiterUser
            {
                Name = model.Name,
                Email = model.Email
            };

            user.Password = GenerateHashPassword(model.Password, user);

            await blogContext.RecruiterUsers.InsertOneAsync(user);
        }

        private string GenerateHashPassword(string password, User user)
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