using MongoDB.Bson;
using WebApp.Helpers;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.Models;
using WebApp.Models.Offer;
using System.Linq;

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
            var recruiter = await dbContext.RecruiterUsers.FindById(r => r.Id == id);
            return recruiter;
        }

        public async Task<CandidateUser> GetCandidateByIdAsync(string id)
        {
            var candidate = await dbContext.CandidateUsers.FindById(r => r.Id == id);
            return candidate;
        }

        public async Task<JobOffer> GetJobOfferByIdAsync(string id)
        {
            var jobOffer = await dbContext.JobOffers.FindById(r => r.Id == id);
            return jobOffer;
        }

        public async Task<List<JobOffer>> GetOffersByIdRecruiterSortedByDateAsync(string id)
        {
            var sortDefinition = GetDateDscSort();
            var offerList = await dbContext.JobOffers.Find(r => r.RecruiterId == id).Sort(sortDefinition).ToListAsync();
            return offerList;
        }

        public async Task<List<JobOffer>> GetOffersByOfferSearchModelAsync(List<Skill> skills, int? minSalary, int? maxSalary, string name, SortBy sortBy)
        {
            var filter = GetSalaryNameFilter(minSalary, maxSalary, name);
            var skillsName = skills.Select(r => r.NameToLower).ToList();
            var matchBson = GetMatchedSkillsStageBson(skillsName);
            var projectBson = GetOfferProjectionBson(skillsName, skills.Count);
            var sortDefinition = GetSortDefinition(sortBy);

            var offersQuery = dbContext.JobOffers
                .Aggregate()
                .Match(filter)
                .Match(new BsonDocumentFilterDefinition<JobOffer>(matchBson))
                .Project(new BsonDocumentProjectionDefinition<JobOffer, BsonDocument>(projectBson))
                .Match(new BsonDocumentFilterDefinition<BsonDocument>(new BsonDocument("Matched", true)))
                .Project(new BsonDocumentProjectionDefinition<BsonDocument, JobOffer>(new BsonDocument("RecruiterId", 1).Add("Salary", 1).Add("Name", 1).Add("Skills", 1).Add("ModificationDate", 1).Add("Description", 1)));

            if (sortDefinition!=null)
            {
                offersQuery = offersQuery.Sort(sortDefinition);
            }
            var offers = await offersQuery.ToListAsync();

            return offers;
        }

        public async Task<List<CandidateUser>> GetCandidatesListBySearchModelAsync(int? minSalary, int? maxSalary, int? minExperienceInYears, int? maxExperienceInYears, SortBy sortBy, List<Skill> skills)
        {
            var filter = GetSalaryExperienceFilter(minSalary, maxSalary, minExperienceInYears, maxExperienceInYears);
            var skillsName = skills.Select(r => r.NameToLower).ToList();
            var matchBson = GetMatchedSkillsStageBson(skillsName);
            var projectBson = GetCandidateProjectionBson(skillsName, skills.Count);
            var sortDefinition = GetCandidateSortDefinition(sortBy);

            var candidatesQuery = dbContext.CandidateUsers
                .Aggregate()
                .Match(filter)
                .Match(new BsonDocumentFilterDefinition<CandidateUser>(matchBson))
                .Project(new BsonDocumentProjectionDefinition<CandidateUser, BsonDocument>(projectBson))
                .Match(new BsonDocumentFilterDefinition<BsonDocument>(new BsonDocument("Matched", true)))
                .Project(new BsonDocumentProjectionDefinition<BsonDocument, CandidateUser>(new BsonDocument("Skills", 1).Add("Salary", 1).Add("ExperienceInYears", 1).Add("Name", 1).Add("ModificationDate",1).Add("ExperienceDescription", 1)));

            if (sortDefinition != null)
            {
                candidatesQuery = candidatesQuery.Sort(sortDefinition);
            }

            var candidates = await candidatesQuery.ToListAsync();
            return candidates;

        }

        public static SortDefinition<CandidateUser> GetCandidateSortDefinition(SortBy sortBy)
        {
            SortDefinition<CandidateUser> sortDefinition = null;
            switch (sortBy)
            {
                case SortBy.SalaryAsc: { sortDefinition = GetSalaryAscCandidateSort(); break; }
                case SortBy.SalaryDsc: { sortDefinition = GetSalaryDscCandidateSort(); break; }
                case SortBy.DateAsc: { sortDefinition = GetDateAscCandidateSort(); break; }
                case SortBy.DateDsc: { sortDefinition = GetDateDscCandidateSort(); break; }
            }
            return sortDefinition;
        }

        public static SortDefinition<JobOffer> GetSortDefinition(SortBy sortBy)
        {
            SortDefinition<JobOffer> sortDefinition = null;
            switch (sortBy)
            {
                case SortBy.SalaryAsc: { sortDefinition = GetSalaryAscSort(); break; }
                case SortBy.SalaryDsc: { sortDefinition = GetSalaryDscSort(); break; }
                case SortBy.DateAsc: { sortDefinition = GetDateAscSort(); break; }
                case SortBy.DateDsc: { sortDefinition = GetDateDscSort(); break; }
            }
            return sortDefinition;
        }

        public static SortDefinition<CandidateUser> GetSalaryAscCandidateSort()
        {
            var sortDefinition = Builders<CandidateUser>.Sort.Ascending("Salary");
            return sortDefinition;
        }

        public static SortDefinition<CandidateUser> GetSalaryDscCandidateSort()
        {
            var sortDefinition = Builders<CandidateUser>.Sort.Descending("Salary");
            return sortDefinition;
        }

        public static SortDefinition<CandidateUser> GetDateAscCandidateSort()
        {
            var sortDefinition = Builders<CandidateUser>.Sort.Ascending("ModificationDate");
            return sortDefinition;
        }

        public static SortDefinition<CandidateUser> GetDateDscCandidateSort()
        {
            var sortDefinition = Builders<CandidateUser>.Sort.Descending("ModificationDate");
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
            var sortDefinition = Builders<JobOffer>.Sort.Ascending("ModificationDate");
            return sortDefinition;
        }

        public static SortDefinition<JobOffer> GetDateDscSort()
        {
            var sortDefinition = Builders<JobOffer>.Sort.Descending("ModificationDate");
            return sortDefinition;
        }

        private static BsonDocument GetSkillsIntersectionProjectionBson(BsonDocument projectBson, List<string> skillsName, int skillsIntersectionCount)
        {
            var bsonArray = GetMapBsonArray(skillsName);
            var intersectbsonDocument = new BsonDocument("$setIntersection", bsonArray);
            var sizebsonDocument = new BsonDocument("$size", intersectbsonDocument);
            var gte = new BsonArray().Add(sizebsonDocument).Add(skillsIntersectionCount);
            projectBson.Add("Matched", new BsonDocument("$gte", gte));
            return projectBson;
        }

        private static BsonDocument GetCandidateProjectionBson(List<string> skillsName, int skillsIntersectionCount)
        {
            BsonDocument projectBson = new BsonDocument("ExperienceInYears", 1);
            projectBson.Add("Salary", 1);
            projectBson.Add("Name", 1);
            projectBson.Add("Skills", 1);
            projectBson.Add("ExperienceDescription", 1);
            projectBson.Add("ModificationDate", 1);
            var skillsIntersection = GetSkillsIntersectionProjectionBson(projectBson, skillsName, skillsIntersectionCount);
            return skillsIntersection;
        }

        private static BsonDocument GetOfferProjectionBson(List<string> skillsName, int skillsIntersectionCount)
        {
            BsonDocument projectBson = new BsonDocument("RecruiterId", 1);
            projectBson.Add("Salary", 1);
            projectBson.Add("Name", 1);
            projectBson.Add("Skills", 1);
            projectBson.Add("ModificationDate", 1);
            projectBson.Add("Description", 1);
            var skillsIntersection = GetSkillsIntersectionProjectionBson(projectBson, skillsName, skillsIntersectionCount);
            return skillsIntersection;
        }

        private static BsonArray GetMapBsonArray(List<string> skillsName)
        {
            var mapbsonDocument = new BsonDocument("input", "$Skills");
            mapbsonDocument.Add("as", "s");
            mapbsonDocument.Add("in", "$$s.NameToLower");
            var bsonArray = new BsonArray();
            bsonArray.Add(new BsonDocument("$map", mapbsonDocument));
            bsonArray.Add(new BsonArray(skillsName));
            return bsonArray;
        }

        private static BsonDocument GetMatchedSkillsStageBson(List<string> skillsName)
        {
            var matchBson = new BsonDocument("Skills.NameToLower", new BsonDocument("$in", new BsonArray(skillsName)));
            return matchBson;
        }

        private static FilterDefinition<CandidateUser> GetSalaryExperienceFilter(int? minSalary, int? maxSalary, int? minExperienceInYears, int? maxExperienceInYears)
        {
            var filterDefinitions = new List<FilterDefinition<CandidateUser>>();
            if (minSalary.HasValue)
            {
                var minFilter = Builders<CandidateUser>.Filter.Where(r => r.Salary >= minSalary);
                filterDefinitions.Add(minFilter);
            }
            if (maxSalary.HasValue)
            {
                var maxFilter = Builders<CandidateUser>.Filter.Where(r => r.Salary < maxSalary);
                filterDefinitions.Add(maxFilter);
            }
            if (minExperienceInYears.HasValue)
            {
                var minExperienceFilter = Builders<CandidateUser>.Filter.Where(r => r.ExperienceInYears >= minExperienceInYears);
                filterDefinitions.Add(minExperienceFilter);
            }
            if (maxExperienceInYears.HasValue)
            {
                var maxExperienceFilter = Builders<CandidateUser>.Filter.Where(r => r.ExperienceInYears < maxExperienceInYears);
                filterDefinitions.Add(maxExperienceFilter);
            }
            var filter = Builders<CandidateUser>.Filter.And(filterDefinitions);
            return filter;
        }

        private static FilterDefinition<JobOffer> GetSalaryNameFilter(int? minSalary, int? maxSalary, string name)
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
                .Set(r => r.Description, offer.Description)
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

        public async Task<List<string>> GetSkillsMatchingQueryAsync(string query)
        {
            var queryToLower = query.ToLower();
            var skillFilterDefinition = Builders<Skill>.Filter.Regex(r => r.NameToLower, new BsonRegularExpression(queryToLower));
            
            var skillNames = await GetSkillNamesFromCandidatesMatchingQueryAsync(queryToLower, skillFilterDefinition);
            var skillNamesFromOffers = await GetSkillNamesFromOffersMatchingQueryAsync(queryToLower, skillFilterDefinition);

            skillNames.AddRange(skillNamesFromOffers);

            return skillNames;
        }

        public async Task<List<string>> GetSkillNamesFromCandidatesMatchingQueryAsync(string queryToLower, FilterDefinition<Skill> skillFilterDefinition)
        {
            var filterFromCandidates = Builders<CandidateUser>.Filter.ElemMatch(user => user.Skills, skillFilterDefinition);
            var skillsFromCandidates = await dbContext.CandidateUsers
                .Find(filterFromCandidates)
                .Project(r => r.Skills.Where(s => s.NameToLower.StartsWith(queryToLower)))
                .ToListAsync();

            var skillNamesFromCandidates = skillsFromCandidates.SelectMany(r => r).Select(r => r.Name).ToList();
            return skillNamesFromCandidates;
        }

        public async Task<List<string>> GetSkillNamesFromOffersMatchingQueryAsync(string queryToLower, FilterDefinition<Skill> skillFilterDefinition)
        {
            var filterFromOffers = Builders<JobOffer>.Filter.ElemMatch(offer => offer.Skills, skillFilterDefinition);
            var skillsFromOffers = await dbContext.JobOffers
                .Find(filterFromOffers)
                .Project(r => r.Skills.Where(s => s.NameToLower.StartsWith(queryToLower)))
                .ToListAsync();

            var skillNamesFromOffers = skillsFromOffers.SelectMany(r => r).Select(r => r.Name).ToList();
            return skillNamesFromOffers;
        }




        public async Task<List<JobOffer>> GetAllOffersListAsync()
        {
            FilterDefinition<JobOffer> filter = "{}";
            var offers = await dbContext.JobOffers.Find(filter).ToListAsync();
            return offers;
        }
    }
}