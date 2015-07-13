using System.Web;

namespace WebApp.Services
{
    public interface IAuthenticationService
    {
        string GetRecruiterIdFromRequest(HttpRequestBase request);
        string GetRoleFromRequest(HttpRequestBase request);
        bool IsAuthenticated(HttpRequestBase request);
        bool IsRecruiter(HttpRequestBase request);
    }
}