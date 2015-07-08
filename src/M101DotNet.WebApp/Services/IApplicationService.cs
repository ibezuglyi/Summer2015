using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.Models;
using WebApp.Models.Account;
using WebApp.Models.Candidate;
using WebApp.Models.Recruiter;

namespace WebApp.Services
{
    public interface IApplicationService
    {
        Task<RecruiterUser> GetRecruterByEmailAsync(string email);
        Task<CandidateUser> GetCandidateByEmailAsync(string email);
        Task CreateRecruiterUserAsync(RegisterModel model);
        string GenerateHashPassword(string password, User user);
        Task CreateCandidateUserAsync(RegisterModel model);
        Task CreateJobOfferAsync(JobOffer model);
        Task<RecruiterUser> GetRecruiterByIdAsync(string id);
        Task<CandidateUser> GetCandidateByIdAsync(string id);
        Task<CandidateUser> UpdateCandidateUserAsync(CandidateUser model, string id);
        Task UpdateRecruiterModelAsync(RecruiterModel model, string id);
        Task<CandidateViewModel> GetCandidateViewModelByIdAsync(string candidateId);
        Task<CandidateViewModel> GetCandidateViewModelByIdAsync(CandidateUserModel candidateModel, string candidateId);
        Task<CandidateUser> UpdateCandidateUserAsync(CandidateUserModel model,string p);
        Task<RecruiterViewModel> GetRecruiterViewModelByIdAsync(string recruiterId);
        Task<List<JobOffer>> GetOffersByIdRecruiterAsync(string idRecruiter);
    }
}