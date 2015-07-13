using System.Security.Claims;
using System.Web;

namespace WebApp.Services
{
    public interface IAuthenticationService
    {
        string GetUserIdFromRequest(HttpRequestBase request);
        string GetRoleFromRequest(HttpRequestBase request);
        bool IsAuthenticated(HttpRequestBase request);
        bool IsRecruiter(HttpRequestBase request);
        bool IsCandidate(HttpRequestBase request);
        void SignIn(ClaimsIdentity identity, HttpRequestBase request);
        void SignOut( HttpRequestBase request);
        bool IfCurrentUserAnOwnerOfOffer(string recruiterIdFromOffer, HttpRequestBase request);
    }
}