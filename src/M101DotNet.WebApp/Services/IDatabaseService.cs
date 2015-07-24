using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.Models;
using WebApp.Models.Offer;
using WebApp.Models.Candidate;
using MongoDB.Driver;

namespace WebApp.Services
{
    public interface IDatabaseService
    {
        Task<RecruiterUser> GetRecruterByEmailAsync(string email);
        Task<CandidateUser> GetCandidateByEmailAsync(string email);
        Task<RecruiterUser> GetRecruiterByIdAsync(string id);
        Task<CandidateUser> GetCandidateByIdAsync(string id);
        Task<JobOffer> GetJobOfferByIdAsync(string id);
        Task<List<JobOffer>> GetOffersByIdRecruiterSortedByDateAsync(string id);
        Task<List<JobOffer>> GetOffersByOfferSearchModelAsync(List<Skill> skills, int? minSalary, int? maxSalary, string name, SortBy sortBy);
        Task UpdateRecruiterAsync(RecruiterUser recruiter, string recruiterId);
        Task UpdateCandidateAsync(CandidateUser candidate, string candidateId);
        Task UpdateJobOfferAsync(JobOffer offer, string offerId);
        Task RemoveJobOfferAsync(string idOffer);
        Task InsertRecruiterUserAsync(RecruiterUser user);
        Task InsertCaniddateUserAsync(CandidateUser user);
        Task InsertJobOfferAsync(JobOffer offer);
        Task<List<string>> GetSkillsMatchingQueryAsync(string query);
        Task<List<JobOffer>> GetAllOffersListAsync();
        Task<List<CandidateUser>> GetCandidatesListBySearchModelAsync(int? minSalary, int? maxSalary, int? minExperienceInYears, int? maxExperienceInYears, SortBy sortBy, List<Skill> skills);
        Task<List<string>> GetSkillNamesFromCandidatesMatchingQueryAsync(string queryToLower, FilterDefinition<Skill> skillFilterDefinition);
        Task<List<string>> GetSkillNamesFromOffersMatchingQueryAsync(string queryToLower, FilterDefinition<Skill> skillFilterDefinition);
    }
}
