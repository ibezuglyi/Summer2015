﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Models.Candidate;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class CandidateProfileController : UserProfileController
    {
        private IApplicationService service;

        public CandidateProfileController(IApplicationService applicationService)
        {
            service = applicationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (IsAuthenticated() && IsCandidate())
            {
                var candidateViewModel = await GetCandidateAsync();
                return View(candidateViewModel);
            }
            return RedirectToAction("DeniedPermision", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string id)
        {
            if (IsAuthenticated() && !IsCandidate())
            {
                var candidateViewModel = await GetCandidateByIdAsync(id);
                return View(candidateViewModel);
            }
            return RedirectToAction("DeniedPermision", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Index(CandidateUserModel model)
        {
            if (IsAuthenticated() && IsCandidate())
            {
                if (ValidateForm(model))
                {
                    await UpdateCandidate(model);
                    var updatedViewModel = await GetCandidateAsync();
                    return View(updatedViewModel);
                }
                var viewModel = await GetCandidateModelAndBindWithStaticAsync(model);
                return View(viewModel);
            }
            return RedirectToAction("DeniedPermission", "Home");
        }

        private bool ValidateForm(CandidateUserModel model)
        {
            if (model.Skills.Count < 1)
            {
                ModelState.AddModelError("notEnoughSkills", "Choose one or more skills");
            }
            if (AreSkillsDuplicate(model))
            {
                ModelState.AddModelError("duplicateSkills", "You can't have repeated skills");
            }
            return ModelState.IsValid;
        }

        private bool AreSkillsDuplicate(CandidateUserModel model)
        {
            var skills = model.Skills;
            var skillsDistinct = model.Skills.Select(r => r.Name).Distinct();
            return skills.Count != skillsDistinct.Count();
        }

        public bool IsCandidate()
        {
            var role = GetRoleFromRequest();
            return role.Value == "Candidate";
        }

        public async Task<CandidateViewModel> GetCandidateAsync()
        {
            var id = GetIdFromRequest();
            var candidateViewModel = await service.GetCandidateViewModelByIdAsync(id.Value);
            return candidateViewModel;
        }

        public async Task<CandidateViewModel> GetCandidateByIdAsync(string candidateId)
        {
            var candidateViewModel = await service.GetCandidateViewModelByIdAsync(candidateId);
            return candidateViewModel;
        }

        public async Task<CandidateViewModel> GetCandidateModelAndBindWithStaticAsync(CandidateUserModel candidateModel)
        {
            var id = GetIdFromRequest();
            var candidateViewModel = await service.GetCandidateViewModelByIdAsync(candidateModel, id.Value);
            return candidateViewModel;
        }

        public async Task UpdateCandidate(CandidateUserModel model)
        {
            var id = GetIdFromRequest();
            await service.UpdateCandidateUserAsync(model, id.Value);
        }
    }
}