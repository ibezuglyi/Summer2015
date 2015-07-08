using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.Models;
using WebApp.Models.Account;
using WebApp.Models.Candidate;

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
        Task<CandidateUser> GetCandidateByIdAsync(string id);
        Task<CandidateUser> UpdateCandidateUserAsync(CandidateUser model, string id);
        Task<CandidateViewModel> GetCandidateViewModelByIdAsync(string candidateId);
    }
}