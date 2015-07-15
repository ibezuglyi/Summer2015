using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.Models;
using WebApp.Models.Candidate;
using WebApp.Models.Offer;
using WebApp.Models.Recruiter;

namespace WebApp.Services
{
    public interface IMappingService
    {
        List<OfferViewModel> MapToOffersViewModel(List<JobOffer> offers);
        OfferListViewModel MapToOfferViewModelList(List<OfferViewModel> offersModelView);
        JobOffer MapToJobOffer(OfferModel model, string id);
        OfferModel MapToOfferModel(JobOffer offer);
        Skill MapToSkill(SkillModel model);
        CandidateUser MapToCandidateUser(CandidateUserModel candidateModel);
        RecruiterUser MapToRecruiterUser(RecruiterModel recruiterModel);
        CandidateUserModel MapToCandidateUserModel(CandidateUser candidate);
        OfferSearchModel MapToOfferSearchModel(CandidateUser candidate);
        RecruiterUser MapToRecruiterUser(string name, string email);
        CandidateUser MapToCandidateUser(string name, string email);
        RecruiterModel MapToRecruiterModel(RecruiterUser recruiter);
        List<Skill> MapSkillModelsToSkills(List<SkillModel> skillModels);
        List<SkillModel> MapSkillsToSkillModels(List<Skill> skills);
        CandidateViewModel MapToCandidateViewModel(CandidateUserModel candidateModel, string candidateName, string candidateEmail);
        RecruiterViewModel MapToRecruiterViewModel(RecruiterModel recruiterModel, string recruiterName, string recruiterEmail);
        OfferViewModel MapToOfferViewModel(OfferModel offerModel, string IdRecruiter);

        SkillSuggestionModel MapToSkillSugestionModel(string query, List<string> hints);

        OfferSearchViewModel MapToOfferSearchViewModel(OfferSearchModel offerSearchModel, ScoredOfferListViewModel scoredOfferListViewModel);
        ScoredOfferViewModel MapToScoredOfferViewModel(ScoredOfferModel offerModel);
        List<ScoredOfferViewModel> MapToScoredOffersViewModel(List<JobOffer> offers);
        ScoredOfferListViewModel MapToScoredOfferListViewModel(List<ScoredOfferViewModel> scoredOffersViewModel);
    }
}
