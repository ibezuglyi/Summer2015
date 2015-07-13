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
        MappingService mapService;
        DatabaseService dbService;

        public ApplicationService()
        {
            mapService = new MappingService();
            dbService = new DatabaseService();
        }
        //do usuniecia - po refaktoryzacji serwisów
        public async Task<RecruiterUser> GetRecruterByEmailAsync(string email)
        {
            var user = await dbService.GetRecruterByEmailAsync(email);
            return user;
        }
        public async Task<CandidateUser> GetCandidateByEmailAsync(string email)
        {
            var user = await dbService.GetCandidateByEmailAsync(email);
            return user;
        }

        public async Task<RecruiterUser> GetRecruiterByIdAsync(string recruiterId)
        {
            var recruiter = await dbService.GetRecruiterByIdAsync(recruiterId);
            return recruiter;
        }

        public async Task<CandidateUser> GetCandidateByIdAsync(string candidateId)
        {
            var candidate = await dbService.GetCandidateByIdAsync(candidateId);
            return candidate;
        }

        public async Task<JobOffer> GetJobOfferByIdAsync(string offerId)
        {
            var jobOffer = await dbService.GetJobOfferByIdAsync(offerId);
            return jobOffer;
        }

        public async Task<List<JobOffer>> GetOffersByIdRecruiterAsync(string recruiterId)
        {
            var offerList = await dbService.GetOffersByIdRecruiterAsync(recruiterId);
            return offerList;
        }

        public async Task<OfferListViewModel> GetOfferViewModelListAsync(string recruiterId)
        {
            var offerList = await GetOffersByIdRecruiterAsync(recruiterId);
            var offersViewModel = mapService.MapToOffersViewModel(offerList); 
            var offerViewModelList = mapService.MapToOfferViewModelList(offersViewModel); 
            return offerViewModelList;
        }

        public async Task CreateRecruiterUserAsync(RegisterModel model)
        {
            var user = mapService.MapToRecruiterUser(model.Name, model.Email);
            user.Password = GenerateHashPassword(model.Password, user);
            await dbService.InsertRecruiterUserAsync(user);
        }

        public async Task CreateJobOfferAsync(OfferModel model, string id)
        {
            var offer = mapService.MapToJobOffer(model, id);
            await dbService.InsertJobOfferAsync(offer);
        }

        public async Task UpdateRecruiterModelAsync(RecruiterModel model, string recruiterId)
        {
            await dbService.UpdateRecruiterModelAsync(model, recruiterId);
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
            var user = mapService.MapToCandidateUser(model.Name, model.Email);
            user.Password = GenerateHashPassword(model.Password, user);
            await dbService.InsertCaniddateUserAsync(user);
        }

        public async Task RemoveJobOfferAsync(string idOffer)
        {
            await dbService.RemoveJobOfferAsync(idOffer);
        }


        public async Task UpdateJobOfferAsync(OfferModel model, string idOffer)
        {
            var offer = mapService.MapToJobOffer(model, idOffer);
            await dbService.UpdateJobOfferAsync(offer, idOffer);
        }

        public async Task UpdateCandidateUserAsync(CandidateUserModel model, string candidateId)
        {
            CandidateUser candidate = mapService.MapToCandidateUser(model);
            await dbService.UpdateCandidateAsync(candidate, candidateId);
        }

        private CandidateUser MapToCandidateUser(CandidateUserModel candidateModel)
        {
            return mapService.MapToCandidateUser(candidateModel);
        }

        //
        public async Task<CandidateViewModel> GetCandidateViewModelByIdAsync(string candidateId)
        {
            var candidateUserModel = mapService.GetCandidateViewModelByIdAsync(string candidateId);
            return candidateUserModel;
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
            var offer = await GetJobOfferByIdAsync(offerId); //db 
            var offerModel = MapToOfferModel(offer); //map
            var offerViewModel = new OfferViewModel(offerModel, offer.IdRecruiter);
            return offerViewModel;
        }
        //przeniesione do mapping service
        private static OfferModel MapToOfferModel(JobOffer offer)
        {
            var skills = MapToSkillModels(offer);
            var offerModel = new OfferModel(offer.Id, offer.Name, offer.Salary, skills);
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
        //-----
    }
}