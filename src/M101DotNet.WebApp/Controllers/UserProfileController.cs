using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class UserProfileController : Controller
    {
        public Claim GetIdFromRequest()
        {
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Sid);
        }

        public Claim GetRoleFromRequest()
        {
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Role);
        }

        public bool IsAuthenticated()
        {
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;
            return authManager.User.Identity.IsAuthenticated;
        }
	}
}