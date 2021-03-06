﻿using Microsoft.Owin.Security;
using System.Linq;
using System.Security.Claims;
using System.Web;
using WebApp.Entities;
using WebApp.Models.Candidate;
using WebApp.Models.Recruiter;

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

        public bool IsCandidate(HttpRequestBase request)
        {
            var role = GetRoleFromRequest(request);
            return role == "Candidate";
        }

        public string GetUserIdFromRequest(HttpRequestBase request)
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


        public void SignIn(ClaimsIdentity identity, HttpRequestBase request)
        {
            var authManager = GetAuthManager(request);
            authManager.SignIn(identity);
        }

        public void SignOut(HttpRequestBase request)
        {
            var authManager = GetAuthManager(request);
            authManager.SignOut("ApplicationCookie");
        }

        public ClaimsIdentity CreateRecruiterIdentity(RecruiterUser user)
        {
            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Sid, user.Id),
                    new Claim(ClaimTypes.Role, "Recruiter"),
                    new Claim(ClaimTypes.Email, user.Email)
                }, "ApplicationCookie");
            return identity;
        }

        public ClaimsIdentity CreateCandidateIdentity(CandidateUser user)
        {
            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Sid, user.Id),
                    new Claim(ClaimTypes.Role, "Candidate"),
                    new Claim(ClaimTypes.Email, user.Email)
                }, "ApplicationCookie");
            return identity;
        }

    }
}