using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.Models;
using WebApp.Models.Account;
using WebApp.Models.Candidate;
using WebApp.Models.Offer;
using WebApp.Models.Recruiter;


namespace WebApp.Services
{
    public class ApplicationService : IApplicationService
    {
        IMappingService _mappingService;
        IDatabaseService _dbService;

        public ApplicationService(IMappingService mappingService, IDatabaseService dbService)
        {
            _mappingService = mappingService;
            _dbService = dbService;
        }

        public async Task<RecruiterUser> GetRecruterByEmailAsync(string email)
        {
            var user = await _dbService.GetRecruterByEmailAsync(email);
            return user;
        }

        public async Task<CandidateUser> GetCandidateByEmailAsync(string email)
        {
            var user = await _dbService.GetCandidateByEmailAsync(email);
            return user;
        }

        public async Task<RecruiterUser> GetRecruiterByIdAsync(string recruiterId)
        {
            var recruiter = await _dbService.GetRecruiterByIdAsync(recruiterId);
            return recruiter;
        }

        public async Task<CandidateUser> GetCandidateByIdAsync(string candidateId)
        {
            var candidate = await _dbService.GetCandidateByIdAsync(candidateId);
            return candidate;
        }

        public async Task<JobOffer> GetJobOfferByIdAsync(string offerId)
        {
            var jobOffer = await _dbService.GetJobOfferByIdAsync(offerId);
            return jobOffer;
        }

        private async Task<List<JobOffer>> GetOffersByIdRecruiterAsync(string recruiterId)
        {
            var offerList = await _dbService.GetOffersByIdRecruiterAsync(recruiterId);
            return offerList;
        }

        public async Task<OfferListViewModel> GetOfferViewModelListAsync(string recruiterId)
        {
            var offerList = await GetOffersByIdRecruiterAsync(recruiterId);
            var offersViewModel = _mappingService.MapToOffersViewModel(offerList);
            var offerViewModelList = _mappingService.MapToOfferViewModelList(offersViewModel);
            return offerViewModelList;
        }

        public async Task<RecruiterModel> GetRecruiterModelByEmailAsync(string email)
        {
            var recruiterUser = await GetRecruterByEmailAsync(email);
            var recruiterModel = _mappingService.MapToRecruiterModel(recruiterUser);
            return recruiterModel;
        }

        public async Task CreateRecruiterUserAsync(RegisterModel model)
        {
            var user = _mappingService.MapToRecruiterUser(model.Name, model.Email);
            user.Password = GenerateHashPassword(model.Password, user);
            await _dbService.InsertRecruiterUserAsync(user);
        }

        public async Task CreateJobOfferAsync(OfferModel model, string offerId)
        {
            var offer = _mappingService.MapToJobOffer(model, offerId);
            await _dbService.InsertJobOfferAsync(offer);
        }

        public async Task UpdateRecruiterModelAsync(RecruiterModel model, string recruiterId)
        {
            var recruiter = _mappingService.MapToRecruiterUser(model);
            await _dbService.UpdateRecruiterAsync(recruiter, recruiterId);
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
            var user = _mappingService.MapToCandidateUser(model.Name, model.Email);
            user.Password = GenerateHashPassword(model.Password, user);
            await _dbService.InsertCaniddateUserAsync(user);
        }

        public async Task RemoveJobOfferAsync(string idOffer)
        {
            await _dbService.RemoveJobOfferAsync(idOffer);
        }

        public async Task UpdateJobOfferAsync(OfferModel model, string idOffer)
        {
            var offer = _mappingService.MapToJobOffer(model, idOffer);
            await _dbService.UpdateJobOfferAsync(offer, idOffer);
        }

        public async Task UpdateCandidateUserAsync(CandidateUserModel model, string candidateId)
        {
            CandidateUser candidate = _mappingService.MapToCandidateUser(model);
            await _dbService.UpdateCandidateAsync(candidate, candidateId);
        }

        public async Task<CandidateViewModel> GetCandidateViewModelByIdAsync(string candidateId)
        {
            var candidate = await _dbService.GetCandidateByIdAsync(candidateId);
            var candidateModel = _mappingService.MapToCandidateUserModel(candidate);
            var candidateViewModel = _mappingService.MapToCandidateViewModel(candidateModel, candidate.Name, candidate.Email);
            return candidateViewModel;
        }

        public async Task<OfferSearchViewModel> GetDefaultOfferSearchViewModelAsync(string candidateId)
        {
            var candidate = await _dbService.GetCandidateByIdAsync(candidateId);
            var offerSearchModel = _mappingService.MapToOfferSearchModel(candidate);
            var offerList = await GetOfferViewModelListAsync(offerSearchModel);
            var offerSearchViewModel = new OfferSearchViewModel(offerSearchModel, offerList);
            return offerSearchViewModel;
        }

        
        public async Task<OfferListViewModel> GetOfferViewModelListAsync(OfferSearchModel offerSearchModel)
        {            
            var offerList = await GetOffersByOfferSearchModelAsync(offerSearchModel);
            var offersViewModel = _mappingService.MapToOffersViewModel(offerList);
            var offerViewModelList = _mappingService.MapToOfferViewModelList(offersViewModel);
            return offerViewModelList;
        }

        private async Task<List<JobOffer>> GetOffersByOfferSearchModelAsync(OfferSearchModel offerSearch)
        {
            var skills = _mappingService.MapSkillModelsToSkills(offerSearch.Skills);
            var offerList = await _dbService.GetOffersByOfferSearchModelAsync(skills, offerSearch.MinSalary, offerSearch.MaxSalary, offerSearch.Name);
            return offerList;
        }

        public async Task<CandidateViewModel> GetCandidateViewModelByIdAsync(CandidateUserModel candidateModel, string candidateId)
        {
            var candidate = await _dbService.GetCandidateByIdAsync(candidateId);
            var candiateViewModel = _mappingService.MapToCandidateViewModel(candidateModel, candidate.Name, candidate.Email);
            return candiateViewModel;
        }


        public async Task<RecruiterViewModel> GetRecruiterViewModelByIdAsync(RecruiterModel recruiterModel, string recruiterId)
        {
            var recruiter = await _dbService.GetRecruiterByIdAsync(recruiterId);
            var recruiterViewModel = _mappingService.MapToRecruiterViewModel(recruiterModel, recruiter.Name, recruiter.Email);
            return recruiterViewModel;
        }

        public async Task<RecruiterViewModel> GetRecruiterViewModelByIdAsync(string recruiterId)
        {
            var recruiter = await _dbService.GetRecruiterByIdAsync(recruiterId);
            var recruiterModel = _mappingService.MapToRecruiterModel(recruiter);
            var recruiterViewModel = _mappingService.MapToRecruiterViewModel(recruiterModel, recruiter.Name, recruiter.Email);
            return recruiterViewModel;
        }

        public async Task<OfferViewModel> GetOfferViewModelByIdAsync(string offerId)
        {
            var offer = await _dbService.GetJobOfferByIdAsync(offerId);
            var offerModel = _mappingService.MapToOfferModel(offer);
            var offerViewModel = _mappingService.MapToOfferViewModel(offerModel, offer.IdRecruiter);
            return offerViewModel;
        }

        public async Task<OfferSearchViewModel> GetOfferSearchViewModelAsync(OfferSearchModel offerSearchModel)
        {
            var offerList = await GetOfferViewModelListAsync(offerSearchModel);
            var offerSearchViewModel = new OfferSearchViewModel(offerSearchModel, offerList);
            return offerSearchViewModel;
        }

        public OfferSearchViewModel GetOfferSearchViewModelWithoutOffersAsync(OfferSearchModel offerSearchModel)
        {
            var offerList = new OfferListViewModel();
            var offerSearchViewModel = new OfferSearchViewModel(offerSearchModel, offerList);
            return offerSearchViewModel;
        }

        public bool AreSkillsDuplicated(List<SkillModel> skills)
        {
            var skillsDistinct = skills.Select(r => r.Name.ToLower()).Distinct();
            return skills.Count != skillsDistinct.Count();
        }

        public bool IsMinSalaryOverMaxSalary(int? minSalary, int? maxSalary)
        {
            if(minSalary.HasValue && maxSalary.HasValue && minSalary.Value>maxSalary.Value)
            {
                return true;
            }
            return false;
        }

        public async Task<List<string>> GetSortedSkillsMatchingQuery(string query)
        {
            var list = await _dbService.GetSkillsMatchingQuery(query);
            var sortedList = list
                .GroupBy<string, string>(r => r)
                .OrderByDescending(group => group.Count())
                .Select(r => r.Key)
                .ToList();
            return sortedList;
        }

        public SkillSuggestionModel MapToSkillSuggestionModel(string query, List<string> hints)
        {
            return _mappingService.MapToSkillSugestionModel(query, hints);
        }

        public async Task<string> GetIdRecruiterByOfferIdAsync(string offerId)
        {
            var offerViewModel = await GetOfferViewModelByIdAsync(offerId);
            return offerViewModel.IdRecruiter;
        }

        public  OfferViewModel GetOfferViewModelAsync(OfferModel offerModel, string recruiterId)
        {
            var offerViewModel = _mappingService.MapToOfferViewModel(offerModel, recruiterId);
            return offerViewModel;
        }

        public double MeasureScoreBetweenCandidateAndOffer(CandidateUser candidateModel, JobOffer offerModel)
        {
            int pointsSum = 0;
            foreach (var candidateSkill in candidateModel.Skills)
            {
                foreach (var offerSkill in offerModel.Skills)
                {
                    if (candidateSkill.Name == offerSkill.Name)
                    {
                        pointsSum += (offerSkill.Level - candidateSkill.Level);
                    }
                }
            }
            double matchFactor = pointsSum / offerModel.Skills.Count;
            return matchFactor;
        }

        public async Task<CandidateUserModel> GetCandidateUserModelByIdAsync(string candidateId)
        {
            var candidate = await GetCandidateByIdAsync(candidateId);
            var candidateModel = _mappingService.MapToCandidateUserModel(candidate);
            return candidateModel;
        }


        //public async Task<OfferListViewModel> GetOffersSortedByMatch(CandidateUser candidate)
        //{
        //    var offerList = await _dbService.GetAllOffersListAsync();
        //    var scoredOfferList = new List<ScoredOfferModel>();
        //    foreach (var offer in offerList){
        //        var score = MeasureScoreBetweenCandidateAndOffer(candidate, offer);
        //        var scoredOffer = _mappingService.MapToScoredOfferModel(offer, score);

        //    }
        //}
    }
}