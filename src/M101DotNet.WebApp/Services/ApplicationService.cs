using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MongoDB.Driver;
using WebApp.Entities;
using WebApp.Models;
using WebApp.Models.Account;
using WebApp.Models.Candidate;

namespace WebApp.Services
{
    public class ApplicationService : IApplicationService
    {
        private JobContext dbContext;

        public ApplicationService()
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

        public async Task<List<JobOffer>> GetOffersByIdRecruiterAsync(string id)
        {
            var offerList = await dbContext.JobOffers.Find(r => r.IdRecruiter == id).ToListAsync();
            return offerList;
        }

        public async Task CreateRecruiterUserAsync(RegisterModel model)
        {
            var user = new RecruiterUser
            {
                Name = model.Name,
                Email = model.Email,
            };

            user.Password = GenerateHashPassword(model.Password, user);
            await dbContext.RecruiterUsers.InsertOneAsync(user);
        }

        public async Task CreateJobOfferAsync(JobOffer model)
        {
            var offer = new JobOffer
            {
                Name = model.Name,
                Salary = model.Salary,
                IdRecruiter = model.IdRecruiter,
                Skills = model.Skills
            };

            await dbContext.JobOffers.InsertOneAsync(offer);
        }

        public async Task<RecruiterUser> UpdateRecruiterUserAsync(RecruiterUser model, string id)
        {
            var filter = Builders<RecruiterUser>.Filter.Eq(r => r.Id, id);
            var update = Builders<RecruiterUser>
                .Update
                .Set(r => r.CompanyDescription, model.CompanyDescription)
                .Set(r => r.CompanyName, model.CompanyName);

            await dbContext.RecruiterUsers.UpdateOneAsync(filter, update);
            var recruiter = await dbContext.RecruiterUsers.Find(r => r.Id == id).SingleOrDefaultAsync();
            return recruiter;
        }


        public string GenerateHashPassword(string password, User user)
        {
            SHA1 sha1 = SHA1.Create();
            string dataToHash = user.Name + password + user.Email;
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(dataToHash));
            StringBuilder returnValue = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            return returnValue.ToString();
        }

        public async Task CreateCandidateUserAsync(RegisterModel model)
        {
            var user = new CandidateUser
            {
                Name = model.Name,
                Email = model.Email,
            };

            user.Password = GenerateHashPassword(model.Password, user);
            await dbContext.CandidateUsers.InsertOneAsync(user);
        }


        public async Task<CandidateUser> UpdateCandidateUserAsync(CandidateUser model, string id)
        {
            var filter = Builders<CandidateUser>.Filter.Eq(r => r.Id, id);
            var update = Builders<CandidateUser>
                .Update
                .Set(r => r.ExperienceDescription, model.ExperienceDescription)
                .Set(r => r.ExperienceInYears, model.ExperienceInYears)
                .Set(r => r.Salary, model.Salary)
                .Set(r => r.Skills, model.Skills);

            await dbContext.CandidateUsers.UpdateOneAsync(filter, update);
            var candidate = await dbContext.CandidateUsers.Find(r => r.Id == id).SingleOrDefaultAsync();
            return candidate;
        }

        public async Task<CandidateViewModel> GetCandidateViewModelByIdAsync(string candidateId)
        {
            var candidate = await GetCandidateByIdAsync(candidateId);
            var candidateModel = MapToCandidateUserModel(candidate);
            var candiateViewModel = new CandidateViewModel(candidateModel, candidate.Name, candidate.Email);
            return candiateViewModel;
        }

        private static CandidateUserModel MapToCandidateUserModel(CandidateUser candidate)
        {
            var skillModels = MapToSkillModels(candidate);
            var candidateModel = new CandidateUserModel(candidate.Salary, candidate.ExperienceDescription, candidate.ExperienceInYears, skillModels);
            return candidateModel;
        }

        private static List<SkillModel> MapToSkillModels(CandidateUser candidate)
        {
            var skillModels = new List<SkillModel>();
            foreach (var skill in candidate.Skills)
            {
                var skillModel = new SkillModel(skill.Name, skill.Level);
                skillModels.Add(skillModel);
            }
            return skillModels;
        }
    }
}