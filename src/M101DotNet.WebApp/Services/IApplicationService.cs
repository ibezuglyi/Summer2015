using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using WebApp.Entities;
using WebApp.Models;
using WebApp.Models.Account;
using WebApp.Models.Candidate;
using WebApp.Models.Offer;
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
        Task CreateJobOfferAsync(OfferModel model, string idRecruiter);        
        Task<RecruiterUser> GetRecruiterByIdAsync(string recruiterId);
        Task<CandidateUser> GetCandidateByIdAsync(string candidateId);
        Task UpdateRecruiterModelAsync(RecruiterModel model, string recruiterId);
        Task<CandidateViewModel> GetCandidateViewModelByIdAsync(string candidateId);
        Task<CandidateViewModel> GetCandidateViewModelByIdAsync(CandidateUserModel candidateModel, string candidateId);
        Task UpdateCandidateUserAsync(CandidateUserModel model, string candidateId);
        Task<RecruiterViewModel> GetRecruiterViewModelByIdAsync(string recruiterId);
        Task<RecruiterViewModel> GetRecruiterViewModelByIdAsync(RecruiterModel recruiterModel, string recruiterId);      
        Task<JobOffer> GetJobOfferByIdAsync(string offerId);
        Task<OfferListViewModel> GetOfferViewModelListAsync(string idRecruiter);        
        Task<OfferViewModel> GetOfferViewModelByIdAsync(string offerId); 
        Task UpdateJobOfferAsync(OfferModel model, string idOffer);
        Task RemoveJobOfferAsync(string idOffer);
        OfferViewModel GetOfferViewModelAsync(OfferModel offerModel, string recruiterId);
        bool AreSkillsDuplicated(List<SkillModel> skills);
        Task<List<string>> GetSortedSkillsMatchingQuery(string query);
        SkillSuggestionModel MapToSkillSuggestionModel(string query, List<string> hints);
        Task<OfferSearchViewModel> GetDefaultOfferSearchViewModelAsync(string candidateId);
        bool IsMinSalaryOverMaxSalary(int? minSalary, int? maxSalary);
        Task<OfferSearchViewModel> GetOfferSearchViewModelAsync(OfferSearchModel offerSearchModel);
        OfferSearchViewModel GetOfferSearchViewModelWithoutOffers(OfferSearchModel offerSearchModel);
        Task<string> GetIdRecruiterByOfferIdAsync(string offerId);
        Task<RecruiterModel> GetRecruiterModelByEmailAsync(string email);
        Task<CandidateUserModel> GetCandidateUserModelByIdAsync(string candidateId);
        Task<ScoredOfferListViewModel> GetOffersSortedByMatch(CandidateUserModel candidate);
    }
}