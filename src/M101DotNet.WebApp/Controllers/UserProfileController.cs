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
        private  Microsoft.Owin.Security.IAuthenticationManager authManager;
        
        public UserProfileController()
        {
            var context = Request.GetOwinContext();
            authManager = context.Authentication;
        }

        public Claim GetIdFromRequest()
        {
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Sid);
        }

        public Claim GetRoleFromRequest()
        {
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Role);
        }

        public bool IsAuthenticated()
        {
            return authManager.User.Identity.IsAuthenticated;
        }
      
	}
}