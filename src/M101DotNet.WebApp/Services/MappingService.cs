﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Entities;
using WebApp.Models;
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
                var offerViewModel = new OfferViewModel(offerModel, offer.RecruiterId, offer.ModificationDate);
                offersViewModel.Add(offerViewModel);
            }
            return offersViewModel;
        }        

        public OfferListViewModel MapToOfferViewModelList(List<OfferViewModel> offersModelView)
        {
            var offerViewModelList = new OfferListViewModel(offersModelView);
            return offerViewModelList;
        }


        public ScoredOfferListViewModel MapToScoredOfferListViewModel(List<ScoredOfferViewModel> scoredOffersViewModel)
        {
            var scoredOfferListViewModel = new ScoredOfferListViewModel(scoredOffersViewModel);
            return scoredOfferListViewModel;
        }

        public  JobOffer MapToJobOffer(OfferModel model, string id)
        {
            var skills = MapSkillModelsToSkills(model.Skills);
            var offer = new JobOffer(model.Name, model.Salary, id, model.Description, skills);
            return offer;
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

        public RecruiterUser MapToRecruiterUser(RecruiterModel recruiterModel)
        {
            var recruiter = new RecruiterUser()
            {
                CompanyDescription = recruiterModel.CompanyDescription,
                CompanyName = recruiterModel.CompanyName,
                
            };
            return recruiter;
        }

        public OfferModel MapToOfferModel(JobOffer offer)
        {
            var skills = MapSkillsToSkillModels(offer.Skills);
            var offerModel = new OfferModel(offer.Id, offer.Name, offer.Salary, offer.Description, skills);
            return offerModel;
        }

        public ScoredOfferModel MapToScoredOfferModel(JobOffer offer, double score)
        {
            var skills = MapSkillsToSkillModels(offer.Skills);
            var scoredOfferModel = new ScoredOfferModel(offer.Id, offer.Name, offer.Salary, score, offer.Description, skills);
            return scoredOfferModel;
        }

        public RecruiterModel MapToRecruiterModel(RecruiterUser recruiter)
        {
            var recruiterModel = new RecruiterModel(recruiter.CompanyName, recruiter.CompanyDescription);
            return recruiterModel;
        }

        public SkillSuggestionModel MapToSkillSugestionModel(string query, List<string> hints)
        {
            var skillSuggestionModel = new SkillSuggestionModel(query, hints);
            return skillSuggestionModel;
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

        public OfferSearchModel MapToOfferSearchModel(CandidateUser candidate)
        {
            var skillModels = MapSkillsToSkillModels(candidate.Skills);
            var offerSearchModel = new OfferSearchModel(skillModels, candidate.Salary);
            return offerSearchModel;
        }

        
        public List<Skill> MapSkillModelsToSkills(List<SkillModel> skillModels)
        {
            if(skillModels == null)
            {
                return new List<Skill>();
            }
            else
            {
                var skills = new List<Skill>();
                foreach (var skillModel in skillModels)
                {
                    var skill = new Skill(skillModel.Name, skillModel.Level);
                    skills.Add(skill);
                }
                return skills;
            }            
        }

        public List<SkillModel> MapSkillsToSkillModels(List<Skill> skills)
        {
            if(skills == null)
            {
                return new List<SkillModel>();
            }
            else
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
        
        public OfferSearchViewModel MapToOfferSearchViewModel(OfferSearchModel offerSearchModel, ScoredOfferListViewModel scoredOfferListViewModel)
        {
            return new OfferSearchViewModel(offerSearchModel, scoredOfferListViewModel);
        }
               

        public CandidateViewModel MapToCandidateViewModel(CandidateUserModel candidateModel, string candidateName, string candidateEmail, DateTime modificationDate)
        {
            return new CandidateViewModel(candidateModel, candidateName, candidateEmail, modificationDate);
        }

        public RecruiterViewModel MapToRecruiterViewModel(RecruiterModel recruiterModel, string recruiterName, string recruiterEmail)
        {
            return new RecruiterViewModel(recruiterModel, recruiterName, recruiterEmail);
        }

        public OfferViewModel MapToOfferViewModel(OfferModel offerModel, string recruiterId, DateTime modificationDate)
        {
            return new OfferViewModel(offerModel, recruiterId, modificationDate);
        }

        public ScoredOfferViewModel MapToScoredOfferViewModel(ScoredOfferModel offerModel, DateTime modificationDate)
        {
            return new ScoredOfferViewModel(offerModel, modificationDate);
        }

        public CandidateSearchViewModel MapToCandidateSearchViewModel(CandidateSearchModel searchModel, ScoredCandidatesListViewModel candidatesListViewModel)
        {
            return new CandidateSearchViewModel(searchModel, candidatesListViewModel);
        }

        public ScoredCandidateModel MapToScoredCandidateModel(CandidateUser candidate, double score)
        {
            var skillModels = MapSkillsToSkillModels(candidate.Skills);
            var scoredCandidateModel = new ScoredCandidateModel(candidate.Name, candidate.Salary, candidate.ExperienceDescription, candidate.ExperienceInYears, score, skillModels);
            return scoredCandidateModel;
        }

        public ScoredCandidateViewModel MapToScoredCandidateViewModel(ScoredCandidateModel scoredCandidateModel, DateTime modificationDate, string candidateId)
        {
            return new ScoredCandidateViewModel(scoredCandidateModel, modificationDate, candidateId);
        }

        public ScoredCandidatesListViewModel MapToScoredCandidatesListViewModel(List<ScoredCandidateViewModel> scoredCandidateViewModelsList)
        {
            return new ScoredCandidatesListViewModel(scoredCandidateViewModelsList);
        }

        public CandidateSearchModel MapToCandidateSearchModel(JobOffer offer)
        {
            if (offer != null)
            {
                var skills = MapSkillsToSkillModels(offer.Skills);
                var candidateSearchModel = new CandidateSearchModel(skills, offer.Salary);
                return candidateSearchModel;
            }
            else
            {
                return new CandidateSearchModel();
            }
        }
    }
}
