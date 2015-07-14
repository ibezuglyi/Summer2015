using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApp.Entities;
using WebApp.Models;
using WebApp.Models.Account;
using MongoDB.Driver;
using WebApp.Models.Offer;
using WebApp.Models.Recruiter;
using WebApp.Models.Candidate;
using MongoDB.Bson;

namespace WebApp.Services
{
    public class DatabaseService : IDatabaseService
    {
        private JobContext dbContext;

        public DatabaseService()
        {
            dbContext = new JobContext();
        }

        public async Task<RecruiterUser> GetRecruterByEmailAsync(string email)
        {
            var user = await dbContext.RecruiterUsers.Find(x => x.Email == email).SingleOrDefaultAsync();
            return user;
        }
        public async Task<CandidateUser> GetCandidateByEmailAsync(string email)
        {
            var user = await dbContext.CandidateUsers.Find(x => x.Email == email).SingleOrDefaultAsync();
            return user;
        }

        public async Task<RecruiterUser> GetRecruiterByIdAsync(string id)
        {
            var recruiter = await dbContext.RecruiterUsers.Find(r => r.Id == id).SingleOrDefaultAsync();
            return recruiter;
        }

        public async Task<CandidateUser> GetCandidateByIdAsync(string id)
        {
            var candidate = await dbContext.CandidateUsers.Find(r => r.Id == id).SingleOrDefaultAsync();
            return candidate;
        }

        public async Task<JobOffer> GetJobOfferByIdAsync(string id)
        {
            var jobOffer = await dbContext.JobOffers.Find(r => r.Id == id).SingleOrDefaultAsync();
            return jobOffer;
        }

        public async Task<List<JobOffer>> GetOffersByIdRecruiterAsync(string id)
        {
            var offerList = await dbContext.JobOffers.Find(r => r.IdRecruiter == id).ToListAsync();
            return offerList;
        }

        public async Task<List<JobOffer>> GetOffersByOfferSearchModelAsync(List<Skill> skills, int? min, int? max, string name)
        {
            //magic

            var offerList = await dbContext.JobOffers.Find(r => 1 == 1).ToListAsync();
            return offerList;
        }

        private static FilterDefinition<JobOffer> GetMinSalaryFilter(int? min)
        {
            FilterDefinition<JobOffer> filterDefinition = null;
            if (min.HasValue)
            {
                filterDefinition = Builders<JobOffer>.Filter.Gt(r => r.Salary, min.Value);
            }
            return filterDefinition;
        }

        public async Task UpdateRecruiterAsync(RecruiterUser recruiter, string recruiterId)
        {
            var filter = Builders<RecruiterUser>.Filter.Eq(r => r.Id, recruiterId);
            var update = Builders<RecruiterUser>
                .Update
                .Set(r => r.CompanyDescription, recruiter.CompanyDescription)
                .Set(r => r.CompanyName, recruiter.CompanyName);

            await dbContext.RecruiterUsers.UpdateOneAsync(filter, update);
        }

        public async Task RemoveJobOfferAsync(string idOffer)
        {
            var filter = Builders<JobOffer>.Filter.Eq(r => r.Id, idOffer);
            await dbContext.JobOffers.DeleteOneAsync(filter);
        }

        public async Task InsertRecruiterUserAsync(RecruiterUser user)
        {
            await dbContext.RecruiterUsers.InsertOneAsync(user);
        }

        public async Task InsertJobOfferAsync(JobOffer offer)
        {
            await dbContext.JobOffers.InsertOneAsync(offer);
        }

        public async Task InsertCaniddateUserAsync(CandidateUser user)
        {
            await dbContext.CandidateUsers.InsertOneAsync(user);
        }

        public async Task UpdateJobOfferAsync(JobOffer offer, string offerId)
        {
            var filter = Builders<JobOffer>.Filter.Eq(r => r.Id, offerId);
            var update = Builders<JobOffer>
                .Update
                .Set(r => r.Name, offer.Name)
                .Set(r => r.Salary, offer.Salary)
                .Set(r => r.Skills, offer.Skills);

            await dbContext.JobOffers.UpdateOneAsync(filter, update);
        }

        public async Task UpdateCandidateAsync(CandidateUser candidate, string candidateId)
        {
            var filter = Builders<CandidateUser>.Filter.Eq(r => r.Id, candidateId);
            var update = Builders<CandidateUser>
                .Update
                .Set(r => r.ExperienceDescription, candidate.ExperienceDescription)
                .Set(r => r.ExperienceInYears, candidate.ExperienceInYears)
                .Set(r => r.Salary, candidate.Salary)
                .Set(r => r.Skills, candidate.Skills);

            await dbContext.CandidateUsers.UpdateOneAsync(filter, update); 
        }
        
        public async Task<List<string>> GetSkillsMatchingQuery(string query)
        {
            var skillFilterDefinition = Builders<Skill>.Filter.Regex(r => r.Name, new BsonRegularExpression(query));
            var filter = Builders<CandidateUser>.Filter.ElemMatch(user => user.Skills, skillFilterDefinition);

            var skills = await dbContext.CandidateUsers
                .Find(filter)
                .Project(r => r.Skills.Where(s => s.Name.Contains(query)))
                .ToListAsync();

            var allSkills = skills.SelectMany(r => r).Select(r => r.Name).ToList();
            return allSkills;
        }
    }
}