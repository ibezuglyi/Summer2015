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

namespace WebApp.Services
{
    public class DatabaseService
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

        public async Task UpdateRecruiterModelAsync(RecruiterModel model, string id)
        {
            var filter = Builders<RecruiterUser>.Filter.Eq(r => r.Id, id);
            var update = Builders<RecruiterUser>
                .Update
                .Set(r => r.CompanyDescription, model.CompanyDescription)
                .Set(r => r.CompanyName, model.CompanyName);

            await dbContext.RecruiterUsers.UpdateOneAsync(filter, update);
        }
    }
}