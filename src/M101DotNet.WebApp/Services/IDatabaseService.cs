using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.Models.Offer;
using WebApp.Models.Recruiter;

namespace WebApp.Services
{
    public interface IDatabaseService
    {
        Task<RecruiterUser> GetRecruterByEmailAsync(string email);
        Task<CandidateUser> GetCandidateByEmailAsync(string email);
        Task<RecruiterUser> GetRecruiterByIdAsync(string id);
        Task<CandidateUser> GetCandidateByIdAsync(string id);
        Task<JobOffer> GetJobOfferByIdAsync(string id);
        Task<List<JobOffer>> GetOffersByIdRecruiterAsync(string id);
        Task<List<JobOffer>> GetOffersByOfferSearchModelAsync(List<Skill> skills, int min, int max, string name);
        Task UpdateRecruiterAsync(RecruiterUser recruiter, string recruiterId);
        Task UpdateCandidateAsync(CandidateUser candidate, string candidateId);
        Task UpdateJobOfferAsync(JobOffer offer, string offerId);
        Task RemoveJobOfferAsync(string idOffer);
        Task InsertRecruiterUserAsync(RecruiterUser user);
        Task InsertCaniddateUserAsync(CandidateUser user);
        Task InsertJobOfferAsync(JobOffer offer);
    }
}
