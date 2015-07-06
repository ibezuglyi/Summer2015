using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Account;

namespace WebApp.Services
{
    public interface IApplicationService
    {
        Task<RecruiterUser> GetRecruterByEmailAsync(string email);
        Task<CandidateUser> GetCandidateByEmailAsync(string email);
        Task CreateRecruiterUserAsync(RegisterModel model);
        string GenerateHashPassword(string password, User user);
        Task CreateCandidateUserAsync(RegisterModel model);
        Task<RecruiterUser> GetRecruiterByIdAsync(string id);
        Task<RecruiterUser> UpdateRecruiterUserAsync(RecruiterUser model, string id);
    }
}