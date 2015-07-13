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
using System.Web.Mvc;
using System.Security.Claims;


namespace WebApp.Services
{
    public class ApplicationService : IApplicationService
    {
        IMappingService mapService;
        IDatabaseService dbService;
        IAuthenticationService authService;

        public ApplicationService(IMappingService MapService, IDatabaseService DbService, IAuthenticationService AuthService)
        {
            mapService = MapService;
            dbService = DbService;
            authService = AuthService;
        }

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

        public async Task CreateJobOfferAsync(OfferModel model, string offerId)
        {
            var offer = mapService.MapToJobOffer(model, offerId);
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

        public async Task<CandidateViewModel> GetCandidateViewModelByIdAsync(string candidateId)
        {
            var candidate = await dbService.GetCandidateByIdAsync(candidateId);
            var candidateModel = mapService.MapToCandidateUserModel(candidate);
            var candidateViewModel = mapService.MapToCandidateViewModel(candidateModel, candidate.Name, candidate.Email);
            return candidateViewModel;
        }

        public async Task<CandidateViewModel> GetCandidateViewModelByIdAsync(CandidateUserModel candidateModel, string candidateId)
        {
            var candidate = await dbService.GetCandidateByIdAsync(candidateId);
            var candiateViewModel = mapService.MapToCandidateViewModel(candidateModel, candidate.Name, candidate.Email);
            return candiateViewModel;
        }


        public async Task<RecruiterViewModel> GetRecruiterViewModelByIdAsync(RecruiterModel recruiterModel, string recruiterId)
        {
            var recruiter = await dbService.GetRecruiterByIdAsync(recruiterId);
            var recruiterViewModel = mapService.MapToRecruiterViewModel(recruiterModel, recruiter.Name, recruiter.Email);
            return recruiterViewModel;
        }

        public async Task<RecruiterViewModel> GetRecruiterViewModelByIdAsync(string recruiterId)
        {
            var recruiter = await dbService.GetRecruiterByIdAsync(recruiterId);
            var recruiterModel = mapService.MapToRecruiterModel(recruiter);
            var recruiterViewModel = mapService.MapToRecruiterViewModel(recruiterModel, recruiter.Name, recruiter.Email);
            return recruiterViewModel;
        }

        public async Task<OfferViewModel> GetOfferViewModelByIdAsync(string offerId)
        {
            var offer = await dbService.GetJobOfferByIdAsync(offerId);
            var offerModel = mapService.MapToOfferModel(offer);
            var offerViewModel = mapService.MapToOfferViewModel(offerModel, offer.IdRecruiter);
            return offerViewModel;
        }

        public bool IsRecruiter(HttpRequestBase request)
        {
            return authService.IsRecruiter(request);
        }

        public bool IsCandidate(HttpRequestBase request)
        {
            return authService.IsCandidate(request);
        }

        public async Task<CandidateViewModel> GetCandidateViewModelAsync(HttpRequestBase request)
        {
            var id = authService.GetUserIdFromRequest(request);
            var candidateViewModel = await GetCandidateViewModelByIdAsync(id);
            return candidateViewModel;
        }

        public async Task<CandidateViewModel> GetCandidateModelAndBindWithStaticAsync(CandidateUserModel candidateModel, HttpRequestBase request)
        {
            var id = authService.GetUserIdFromRequest(request);
            var candidateViewModel = await GetCandidateViewModelByIdAsync(candidateModel, id);
            return candidateViewModel;
        }

        public async Task UpdateCandidate(CandidateUserModel model, HttpRequestBase request)
        {
            var id = authService.GetUserIdFromRequest(request);
            await UpdateCandidateUserAsync(model, id);
        }

        public Task<RecruiterViewModel> GetRecruiterViewModelAsync(HttpRequestBase request)
        {
            var id = authService.GetUserIdFromRequest(request);
            var recruiterModel = GetRecruiterViewModelByIdAsync(id);
            return recruiterModel;
        }

        public Task<RecruiterViewModel> GetRecruiterViewModelAsync(RecruiterModel recruiterModel, HttpRequestBase request)
        {
            var id = authService.GetUserIdFromRequest(request);
            var recruiterViewModel = GetRecruiterViewModelByIdAsync(recruiterModel, id);
            return recruiterViewModel;
        }

        public async Task UpdateRecruiter(RecruiterModel model, HttpRequestBase request)
        {
            var id = authService.GetUserIdFromRequest(request);
            await UpdateRecruiterModelAsync(model, id);
        }

        public bool IsAuthenticated(HttpRequestBase request)
        {
            return authService.IsAuthenticated(request);
        }

        public Task<OfferListViewModel> GetRecruiterOfferListViewModelAsync(HttpRequestBase request)
        {
            var recruiterId = authService.GetUserIdFromRequest(request);
            var offersRecruiter = GetOfferViewModelListAsync(recruiterId);
            return offersRecruiter;
        }

        public async Task CreateJobOfferForRecruiter(OfferModel model, HttpRequestBase request)
        {
            var recruiterId = authService.GetUserIdFromRequest(request);
            await CreateJobOfferAsync(model, recruiterId);
        }

        public bool IfCurrentUserAnOwnerOfOffer(string recruiterIdFromOffer, HttpRequestBase request)
        {
            return authService.IfCurrentUserAnOwnerOfOffer(recruiterIdFromOffer, request);
        }


        public  OfferViewModel GetOfferViewModelAsync(OfferModel offerModel, HttpRequestBase request)
        {
            var recruiterId = authService.GetUserIdFromRequest(request);
            var offerViewModel = mapService.MapToOfferViewModel(offerModel, recruiterId);
            return offerViewModel;
        }


        public bool AreSkillsDuplicated(List<SkillModel> skills)
        {
            var skillsDistinct = skills.Select(r => r.Name.ToLower()).Distinct();
            return skills.Count != skillsDistinct.Count();
        }

        public void SignOut(HttpRequestBase request)
        {
            authService.SignOut(request);
        }

        public void SignIn(ClaimsIdentity identity, HttpRequestBase request)
        {
            authService.SignIn(identity, request);
        }

        public ClaimsIdentity CreateRecruiterIdentity(RecruiterUser user)
        {
            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Sid, user.Id),
                    new Claim(ClaimTypes.Role, "Recruiter"),
                    new Claim(ClaimTypes.Email, user.Email)
                }, "ApplicationCookie");
            return identity;
        }

        public ClaimsIdentity CreateCandidateIdentity(CandidateUser user)
        {
            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Sid, user.Id),
                    new Claim(ClaimTypes.Role, "Candidate"),
                    new Claim(ClaimTypes.Email, user.Email)
                }, "ApplicationCookie");
            return identity;
        }
    }
}