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
    public class MappingService : IMappingService
    {
        public List<OfferViewModel> MapToOffersViewModel(List<JobOffer> offers)
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

        public OfferListViewModel MapToOfferViewModelList(List<OfferViewModel> offersModelView)
        {
            var offerViewModelList = new OfferListViewModel(offersModelView);
            return offerViewModelList;
        }

        public  JobOffer MapToJobOffer(OfferModel model, string id)
        {
            var skills = MapSkillModelsToSkills(model.Skills);
            var offer = new JobOffer(model.Name, model.Salary, id, skills);
            return offer;
        }

        public Skill MapToSkill(SkillModel model)
        {
            var skill = new Skill
            {
                Name = model.Name,
                Level = model.Level,
            };
            return skill;
        }

        public CandidateUser MapToCandidateUser(CandidateUserModel candidateModel)
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

         public OfferModel MapToOfferModel(JobOffer offer)
        {
            var skills = MapSkillsToSkillModels(offer.Skills);
            var offerModel = new OfferModel(offer.Id, offer.Name, offer.Salary, skills);
            return offerModel;
        }

        public RecruiterModel MapToRecruiterModel(RecruiterUser recruiter)
        {
            var recruiterModel = new RecruiterModel(recruiter.CompanyName, recruiter.CompanyDescription);
            return recruiterModel;
        }

        public RecruiterUser MapToRecruiterUser(string name, string email)
        {
            var user = new RecruiterUser 
            {
                Name = name,
                Email = email,
            };
            return user;
        }

        public  CandidateUser MapToCandidateUser(string name, string email)
        {
            var user = new CandidateUser
            {
                Name = name,
                Email = email,
            };
            return user;
        }

        public  CandidateUserModel MapToCandidateUserModel(CandidateUser candidate)
        {
            var skillModels = MapSkillsToSkillModels(candidate.Skills);
            var candidateModel = new CandidateUserModel(candidate.Salary, candidate.ExperienceDescription, candidate.ExperienceInYears, skillModels);
            return candidateModel;
        }


        public List<Skill> MapSkillModelsToSkills(List<SkillModel> skillModels)
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

        public List<SkillModel> MapSkillsToSkillModels(List<Skill> skills)
        {
            var skillModels = new List<SkillModel>();
            foreach (var skill in skills)
            {
                var skillModel = new SkillModel(skill.Name, skill.Level);
                skillModels.Add(skillModel);
            }
            return skillModels;
        }

        public CandidateViewModel MapToCandidateViewModel(CandidateUserModel candidateModel, string candidateName, string candidateEmail)
        {
            return new CandidateViewModel(candidateModel, candidateName, candidateEmail);
        }

        public RecruiterViewModel MapToRecruiterViewModel(RecruiterModel recruiterModel, string recruiterName, string recruiterEmail)
        {
            return new RecruiterViewModel(recruiterModel, recruiterName, recruiterEmail);
        }

        public OfferViewModel MapToOfferViewModel(OfferModel offerModel, string IdRecruiter)
        {
            return new OfferViewModel(offerModel, IdRecruiter);
        }
    }
}
