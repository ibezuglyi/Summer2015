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
using WebApp.Models.Recruiter;
using WebApp.Models.Offer;

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

        public async Task<OfferViewModelList> GetOfferViewModelListAsync(string id)
        {
            var offerList = await GetOffersByIdRecruiterAsync(id);
            var offersViewModel = MapToOffersViewModel(offerList);
            var offerViewModelList = MapToOfferViewModelList(offersViewModel);
            return offerViewModelList;
        }

        private static List<OfferViewModel> MapToOffersViewModel(List<JobOffer> offers)
        {
            var offersViewModel = new List<OfferViewModel>();
            foreach(var offer in offers)
            {
                var offerModel = MapToOfferModel(offer);
                var offerViewModel = new OfferViewModel(offerModel, offer.Id, offer.IdRecruiter);
                offersViewModel.Add(offerViewModel);
            }
            return offersViewModel;
        }

        private static OfferViewModelList MapToOfferViewModelList(List<OfferViewModel> offersModelView)
        {
            var offerViewModelList = new OfferViewModelList(offersModelView);
            return offerViewModelList;
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

        public async Task CreateJobOfferAsync(OfferModel model, string id)
        {
            var offer = MapToJobOffer(model, id);
            await dbContext.JobOffers.InsertOneAsync(offer);
        }

        public static JobOffer MapToJobOffer(OfferModel model, string id)
        {
            var skills = MapToSkills(model);
            var offer = new JobOffer(model.Name, model.Salary, id, skills);
            return offer;
        }

        public static List<Skill> MapToSkills(OfferModel model)
        {
            List<Skill> skills = new List<Skill>();
            foreach(var modelSkill in model.Skills)
            {
                var skill = MapToSkill(modelSkill);
                skills.Add(skill);
            }
            return skills;
        }

        public static Skill MapToSkill(SkillModel model)
        {
            var skill = new Skill
            {
                Name = model.Name,
                Level = model.Level,
            };
            return skill;
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


        public async Task UpdateCandidateUserAsync(CandidateUserModel model, string id)
        {
            CandidateUser candidate = MapToCandidateUser(model);
            var filter = Builders<CandidateUser>.Filter.Eq(r => r.Id, id);
            var update = Builders<CandidateUser>
                .Update
                .Set(r => r.ExperienceDescription, candidate.ExperienceDescription)
                .Set(r => r.ExperienceInYears, candidate.ExperienceInYears)
                .Set(r => r.Salary, candidate.Salary)
                .Set(r => r.Skills, candidate.Skills);

            await dbContext.CandidateUsers.UpdateOneAsync(filter, update);
        }

        private static CandidateUser MapToCandidateUser(CandidateUserModel candidateModel)
        {
            var skills = MapToSkills(candidateModel);
            var candidate = new CandidateUser()
            {
                ExperienceDescription = candidateModel.ExperienceDescription,
                ExperienceInYears = candidateModel.ExperienceInYears,
                Salary = candidateModel.Salary,
                Skills = skills,
            };
            return candidate;
        }

        public async Task<CandidateViewModel> GetCandidateViewModelByIdAsync(string candidateId)
        {
            var candidate = await GetCandidateByIdAsync(candidateId);
            var candidateModel = MapToCandidateUserModel(candidate);
            var candiateViewModel = new CandidateViewModel(candidateModel, candidate.Name, candidate.Email);
            return candiateViewModel;
        }

        public async Task<CandidateViewModel> GetCandidateViewModelByIdAsync(CandidateUserModel candidateModel, string candidateId)
        {
            var candidate = await GetCandidateByIdAsync(candidateId);
            var candiateViewModel = new CandidateViewModel(candidateModel, candidate.Name, candidate.Email);
            return candiateViewModel;
        }

        public async Task<RecruiterViewModel> GetRecruiterViewModelByIdAsync(RecruiterModel recruiterModel, string recruiterId)
        {
            var recruiter = await GetRecruiterByIdAsync(recruiterId);
            var recruiterViewModel = new RecruiterViewModel(recruiterModel, recruiter.Name, recruiter.Email);
            return recruiterViewModel;
        }

        public async Task<RecruiterViewModel> GetRecruiterViewModelByIdAsync(string recruiterId)
        {
            var recruiter = await GetRecruiterByIdAsync(recruiterId);
            var recruiterModel = MapToRecruiterModel(recruiter);
            var recruiterViewModel = new RecruiterViewModel(recruiterModel, recruiter.Name, recruiter.Email);
            return recruiterViewModel;
        }

        public async Task<OfferViewModel> GetOfferViewModelByIdAsync(string offerId)
        {
            var offer = await GetJobOfferByIdAsync(offerId);
            var offerModel = MapToOfferModel(offer);
            var offerViewModel = new OfferViewModel(offerModel, offer.Id, offer.IdRecruiter);
            return offerViewModel;
        }

        private static OfferModel MapToOfferModel(JobOffer offer)
        {
            var skills = MapToSkillModels(offer);
            var offerModel = new OfferModel(offer.Name, offer.Salary, skills);
            return offerModel;
        }


        private static RecruiterModel MapToRecruiterModel(RecruiterUser recruiter)
        {
            var recruiterModel = new RecruiterModel(recruiter.CompanyName, recruiter.CompanyDescription);
            return recruiterModel;
        }

        private static CandidateUserModel MapToCandidateUserModel(CandidateUser candidate)
        {
            var skillModels = MapToSkillModels(candidate);
            var candidateModel = new CandidateUserModel(candidate.Salary, candidate.ExperienceDescription, candidate.ExperienceInYears, skillModels);
            return candidateModel;
        }

        //I don't like it
        private static List<SkillModel> MapToSkillModels(JobOffer offer)
        {
            var skillModels = new List<SkillModel>();
            foreach (var skill in offer.Skills)
            {
                var skillModel = new SkillModel(skill.Name, skill.Level);
                skillModels.Add(skillModel);
            }
            return skillModels;
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

        private static List<Skill> MapToSkills(CandidateUserModel candidate)
        {
            var skills = new List<Skill>();
            foreach (var skillModel in candidate.Skills)
            {
                var skill = new Skill()
        {
                    Name = skillModel.Name,
                    Level = skillModel.Level,
                };
                skills.Add(skill);
            }
            return skills;
        }

    }
}