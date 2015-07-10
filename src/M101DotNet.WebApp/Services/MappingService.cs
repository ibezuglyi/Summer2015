using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Entities;
using WebApp.Models.Candidate;
using WebApp.Models.Offer;
using WebApp.Models.Recruiter;

namespace WebApp.Services
{
    public class MappingService
    {
        private static List<OfferViewModel> MapToOffersViewModel(List<JobOffer> offers)
        {
            var offersViewModel = new List<OfferViewModel>();
            foreach (var offer in offers)
            {
                var offerModel = MapToOfferModel(offer);
                var offerViewModel = new OfferViewModel(offerModel, offer.IdRecruiter);
                offersViewModel.Add(offerViewModel);
            }
            return offersViewModel;
        }

        private static OfferListViewModel MapToOfferViewModelList(List<OfferViewModel> offersModelView)
        {
            var offerViewModelList = new OfferListViewModel(offersModelView);
            return offerViewModelList;
        }

        public static JobOffer MapToJobOffer(OfferModel model, string id)
        {
            var skills = MapSkillModelsToSkills(model.Skills);
            var offer = new JobOffer(model.Name, model.Salary, id, skills);
            return offer;
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

        private static CandidateUser MapToCandidateUser(CandidateUserModel candidateModel)
        {
            var skills = MapSkillModelsToSkills(candidateModel.Skills);
            var candidate = new CandidateUser()
            {
                ExperienceDescription = candidateModel.ExperienceDescription,
                ExperienceInYears = candidateModel.ExperienceInYears,
                Salary = candidateModel.Salary,
                Skills = skills,
            };
            return candidate;
        }

         private static OfferModel MapToOfferModel(JobOffer offer)
        {
            var skills = MapSkillsToSkillModels(offer.Skills);
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
            var skillModels = MapSkillsToSkillModels(candidate.Skills);
            var candidateModel = new CandidateUserModel(candidate.Salary, candidate.ExperienceDescription, candidate.ExperienceInYears, skillModels);
            return candidateModel;
        }

        private static List<Skill> MapSkillModelsToSkills(List<SkillModel> skillModels)
        {
            var skills = new List<Skill>();
            foreach(var skillModel in skillModels)
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

        private static List<SkillModel> MapSkillsToSkillModels(List<Skill> skills)
        {
            var skillModels = new List<SkillModel>();
            foreach (var skill in skills)
            {
                var skillModel = new SkillModel(skill.Name, skill.Level);
                skillModels.Add(skillModel);
            }
            return skillModels;
        }
    }
}
