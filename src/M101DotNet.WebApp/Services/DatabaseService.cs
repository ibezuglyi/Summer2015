using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.Models;

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

        public async Task<List<JobOffer>> GetOffersByOfferSearchModelAsync(List<Skill> skills, int? minSalary, int? maxSalary, string name, string sortBy)
        {
            var filter = GetSimpleTypePartFilter(minSalary, maxSalary, name);
            var skillsName = skills.Select(r => r.Name).ToList();
            var matchBson = GetMatchedSkillsStageBson(skillsName);
            var projectBson = GetSkillsIntersectionProjectionBson(skillsName, skills.Count);
            var sortDefinition = GetSortDefinition(sortBy);

            var offersQuery = dbContext.JobOffers
                .Aggregate()
                .Match(filter)
                .Match(new BsonDocumentFilterDefinition<JobOffer>(matchBson))
                .Project(new BsonDocumentProjectionDefinition<JobOffer, BsonDocument>(projectBson))
                .Match(new BsonDocumentFilterDefinition<BsonDocument>(new BsonDocument("Matched", true)))
                .Project(new BsonDocumentProjectionDefinition<BsonDocument, JobOffer>(new BsonDocument("IdRecruiter", 1).Add("Salary", 1).Add("Name", 1).Add("Skills", 1)));

            if (sortDefinition!=null)
            {
                offersQuery = offersQuery.Sort(sortDefinition);
            }
            var offers = await offersQuery.ToListAsync();

            return offers;
        }

        public static SortDefinition<JobOffer> GetSortDefinition(string sortBy)
        {
            SortDefinition<JobOffer> sortDefinition = null;
            switch (sortBy)
            {
                case "salaryAsc": { sortDefinition = GetSalaryAscSort(); break; }
                case "salaryDsc": { sortDefinition = GetSalaryDscSort(); break; }
                case "dateAsc": { sortDefinition = GetDateAscSort(); break; }
                case "dateDsc": { sortDefinition = GetDateDscSort(); break; }
            }
            return sortDefinition;
        }

        public static SortDefinition<JobOffer> GetSalaryAscSort()
        {
            var sortDefinition = Builders<JobOffer>.Sort.Ascending("Salary");
            return sortDefinition;
        }

        public static SortDefinition<JobOffer> GetSalaryDscSort()
        {
            var sortDefinition = Builders<JobOffer>.Sort.Descending("Salary");
            return sortDefinition;
        }

        public static SortDefinition<JobOffer> GetDateAscSort()
        {
            var sortDefinition = Builders<JobOffer>.Sort.Ascending("ModyficationDate");
            return sortDefinition;
        }

        public static SortDefinition<JobOffer> GetDateDscSort()
        {
            var sortDefinition = Builders<JobOffer>.Sort.Descending("ModyficationDate");
            return sortDefinition;
        }

        private static BsonDocument GetSkillsIntersectionProjectionBson(List<string> skillsName, int skillsIntersectionCount)
        {
            BsonDocument projectBson = new BsonDocument("IdRecruiter", 1);
            projectBson.Add("Salary", 1);
            projectBson.Add("Name", 1);
            projectBson.Add("Skills", 1);

            var bsonArray = GetMapBsonArray(skillsName);
            var intersectbsonDocument = new BsonDocument("$setIntersection", bsonArray);
            var sizebsonDocument = new BsonDocument("$size", intersectbsonDocument);
            var gte = new BsonArray().Add(sizebsonDocument).Add(skillsIntersectionCount);
            projectBson.Add("Matched", new BsonDocument("$gte", gte));
            return projectBson;
        }

        private static BsonArray GetMapBsonArray(List<string> skillsName)
        {
            var mapbsonDocument = new BsonDocument("input", "$Skills");
            mapbsonDocument.Add("as", "s");
            mapbsonDocument.Add("in", "$$s.Name");
            var bsonArray = new BsonArray();
            bsonArray.Add(new BsonDocument("$map", mapbsonDocument));
            bsonArray.Add(new BsonArray(skillsName));
            return bsonArray;
        }

        private static BsonDocument GetMatchedSkillsStageBson(List<string> skillsName)
        {
            var matchBson = new BsonDocument("Skills.Name", new BsonDocument("$in", new BsonArray(skillsName)));
            return matchBson;
        }

        private static FilterDefinition<JobOffer> GetSimpleTypePartFilter(int? minSalary, int? maxSalary, string name)
        {
            var filterDefinitions = new List<FilterDefinition<JobOffer>>();
            if (minSalary.HasValue)
            {
                var minFilter = GetMinSalaryFilter(minSalary.Value);
                filterDefinitions.Add(minFilter);
            }
            if (maxSalary.HasValue)
            {
                var maxFilter = GetMaxSalaryFilter(maxSalary.Value);
                filterDefinitions.Add(maxFilter);
            }
            if (name != null)
            {
                var nameFilter = GetNameFilter(name);
                filterDefinitions.Add(nameFilter);
            }
            var filter = Builders<JobOffer>.Filter.And(filterDefinitions);
            return filter;
        }

        private static FilterDefinition<JobOffer> GetNameFilter(string name)
        {
            var nameFilter = Builders<JobOffer>.Filter.Where(r => r.Name.ToLower() == name.ToLower());
            return nameFilter;
        }

        private static FilterDefinition<JobOffer> GetMaxSalaryFilter(int max)
        {
            var maxFilter = Builders<JobOffer>.Filter.Where(r => r.Salary <= max);
            return maxFilter;
        }

        private static FilterDefinition<JobOffer> GetMinSalaryFilter(int min)
        {
            var minFilter = Builders<JobOffer>.Filter.Where(r => r.Salary >= min);
            return minFilter;
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
                .Set(r => r.Skills, offer.Skills)
                .Set(r => r.ModificationDate, offer.ModificationDate);

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
                .Set(r => r.Skills, candidate.Skills)
                .Set(r => r.ModificationDate, candidate.ModificationDate);

            await dbContext.CandidateUsers.UpdateOneAsync(filter, update);
        }

        public async Task<List<string>> GetSkillsMatchingQuery(string query)
        {
            var queryToLower = query.ToLower();
            var skillFilterDefinition = Builders<Skill>.Filter.Regex(r => r.NameToLower, new BsonRegularExpression(queryToLower));
            var filter = Builders<CandidateUser>.Filter.ElemMatch(user => user.Skills, skillFilterDefinition);

            var skills = await dbContext.CandidateUsers
                .Find(filter)
                .Project(r => r.Skills.Where(s => s.NameToLower.StartsWith(queryToLower)))
                .ToListAsync();

            var skillNames = skills.SelectMany(r => r).Select(r => r.Name).ToList();
            return skillNames;
        }


        public async Task<List<JobOffer>> GetAllOffersListAsync()
        {
            FilterDefinition<JobOffer> filter = "{}";
            var offers = await dbContext.JobOffers.Find(filter).ToListAsync();
            return offers;
        }
    }
}