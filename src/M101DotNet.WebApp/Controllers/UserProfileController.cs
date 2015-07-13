using Microsoft.Owin.Security;
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
            var authManager = GetAuthManager();
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Sid);
        }

        public Claim GetRoleFromRequest()
        {
            var authManager = GetAuthManager();
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Role);
        }

        public bool IsAuthenticated()
        {
            var authManager = GetAuthManager();
            return authManager.User.Identity.IsAuthenticated;
        }
      
        public IAuthenticationManager GetAuthManager()
        {
            var context = Request.GetOwinContext();
            return context.Authentication;
        }
	}
}