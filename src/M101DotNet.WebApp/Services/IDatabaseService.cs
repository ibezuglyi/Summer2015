using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Entities;

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
        Task<List<JobOffer>> GetOffersByOfferSearchModelAsync(List<Skill> skills, int? minSalary, int? maxSalary, string name);
        Task UpdateRecruiterAsync(RecruiterUser recruiter, string recruiterId);
        Task UpdateCandidateAsync(CandidateUser candidate, string candidateId);
        Task UpdateJobOfferAsync(JobOffer offer, string offerId);
        Task RemoveJobOfferAsync(string idOffer);
        Task InsertRecruiterUserAsync(RecruiterUser user);
        Task InsertCaniddateUserAsync(CandidateUser user);
        Task InsertJobOfferAsync(JobOffer offer);

        Task<List<string>> GetSkillsMatchingQuery(string query);
        Task<List<JobOffer>> GetAllOffersListAsync();
        Task InsertScoredSkillRelationsAsync(List<ScoredSkillRelation> scoredSkillRelations);
        Task<List<CandidateUser>>  GetAllCandidatesListAsync();
        Task<List<JobOffer>> SearchForOffers(string skillcode);
    }
}
