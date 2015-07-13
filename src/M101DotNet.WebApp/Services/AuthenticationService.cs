﻿using Microsoft.Owin.Security;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace WebApp.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public bool IsAuthenticated(HttpRequestBase request)
        {
            var authManager = GetAuthManager(request);
            return authManager.User.Identity.IsAuthenticated;
        }

        private IAuthenticationManager GetAuthManager(HttpRequestBase request)
        {
            var context = request.GetOwinContext();
            return context.Authentication;
        }
        public string GetRoleFromRequest(HttpRequestBase request)
        {
            var authManager = GetAuthManager(request);
            var claim = authManager.User.Claims.SingleOrDefault(r => r.Type == ClaimTypes.Role);
            return GetSafeClaimValue(claim);
        }
        
        public bool IsRecruiter(HttpRequestBase request)
        {
            var role = GetRoleFromRequest(request);
            return role == "Recruiter";
        }

        public string GetRecruiterIdFromRequest(HttpRequestBase request)
        {
            var authManager = GetAuthManager(request);
            var claim = authManager.User.Claims.SingleOrDefault(r => r.Type == ClaimTypes.Sid);
            return GetSafeClaimValue(claim);
        }

        private static string GetSafeClaimValue(Claim claim)
        {
            if (claim != null)
            {
                return claim.Value;
            }
            return string.Empty;
        }
    }
}