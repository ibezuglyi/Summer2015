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

        Task<OfferSearchViewModel> GetDefaultOfferSearchViewModel(HttpRequestBase request);
        Task<OfferSearchViewModel> GetDefaultOfferSearchViewModel(string candidateId);

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
        bool IsRecruiter(HttpRequestBase request);
        bool IsCandidate(HttpRequestBase request);
        Task<CandidateViewModel> GetCandidateViewModelAsync(HttpRequestBase request);
        Task<CandidateViewModel> GetCandidateModelAndBindWithStaticAsync(CandidateUserModel candidateModel, HttpRequestBase request);
        Task UpdateCandidate(CandidateUserModel model, HttpRequestBase request);
        Task<RecruiterViewModel> GetRecruiterViewModelAsync(HttpRequestBase request);
        Task<RecruiterViewModel> GetRecruiterViewModelAsync(RecruiterModel recruiterModel, HttpRequestBase request);
        Task UpdateRecruiter(RecruiterModel model, HttpRequestBase request);
        bool IsAuthenticated(HttpRequestBase request);
        Task<OfferListViewModel> GetRecruiterOfferListViewModelAsync(HttpRequestBase request);
        Task CreateJobOfferForRecruiter(OfferModel model, HttpRequestBase request);
        bool IfCurrentUserAnOwnerOfOffer(string recruiterIdFromOffer, HttpRequestBase request);
        OfferViewModel GetOfferViewModelAsync(OfferModel offerModel, HttpRequestBase request);
        bool AreSkillsDuplicated(List<SkillModel> skills);
        void SignOut(HttpRequestBase request);
        void SignIn(ClaimsIdentity identity, HttpRequestBase request);
        ClaimsIdentity CreateRecruiterIdentity(RecruiterUser user);
        ClaimsIdentity CreateCandidateIdentity(CandidateUser user);
        
    }
}