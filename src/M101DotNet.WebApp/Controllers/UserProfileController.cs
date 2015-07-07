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
            var authManager = getAuthManager();
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Sid);
        }

        public Claim GetRoleFromRequest()
        {
            var authManager = getAuthManager();
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Role);
        }

        public bool IsAuthenticated()
        {
            var authManager = getAuthManager();
            return authManager.User.Identity.IsAuthenticated;
        }
      
        public Microsoft.Owin.Security.IAuthenticationManager getAuthManager()
        {
            var context = Request.GetOwinContext();
            return context.Authentication;
        }
	}
}