using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.Models.Candidate;
using WebApp.Models.Offer;
using WebApp.Models.Recruiter;

namespace WebApp.Services
{
    interface IMappingService
    {
        List<OfferViewModel> MapToOffersViewModel(List<JobOffer> offers);
        OfferListViewModel MapToOfferViewModelList(List<OfferViewModel> offersModelView);
        JobOffer MapToJobOffer(OfferModel model, string id);
        OfferModel MapToOfferModel(JobOffer offer);
        Skill MapToSkill(SkillModel model);
        CandidateUser MapToCandidateUser(CandidateUserModel candidateModel);
        CandidateUserModel MapToCandidateUserModel(CandidateUser candidate);
        RecruiterUser MapToRecruiterUser(string name, string email);
        CandidateUser MapToCandidateUser(string name, string email);
        RecruiterModel MapToRecruiterModel(RecruiterUser recruiter);
        List<Skill> MapSkillModelsToSkills(List<SkillModel> skillModels);
        List<SkillModel> MapSkillsToSkillModels(List<Skill> skills);
    }
}
