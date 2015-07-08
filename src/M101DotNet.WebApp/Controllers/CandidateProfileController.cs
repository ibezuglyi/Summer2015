using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Entities;
using WebApp.Models;
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
            if (IsAuthenticated())
            {
                var role = GetRoleFromRequest();
                if (role.Value == "Candidate")
                {
                    var candidateViewModel = await GetCandidateAsync();
                    return View(candidateViewModel);
                }
            }

            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public async Task<ActionResult> Index(CandidateUserModel model)
        {
            var viewModel = await GetCandidateModelAndBindWithStaticAsync(model);
            //if (ValidateForm(model))
            //{
            //    var updatedModel = await UpdateCandidate(model);
            //    return View(updatedModel);
            //}

            return View(viewModel);

        }

        private bool ValidateForm(CandidateUser model)
        {
            if (model.Skills.Count < 1)
            {
                AddWrongNumberOfSkillsError("notEnoughSkills");
            }

            var isduplicated = model.Skills.Count != model.Skills.Select(r => r.Name.ToLower()).Distinct().Count();
            if (isduplicated)
            {
                AddDuplicateSkillError("duplicateSkills");
            }
            return ModelState.IsValid;
        }


        private void AddWrongNumberOfSkillsError(string field)
        {
            ModelState.AddModelError(field, "Choose one or more skills");
        }

        public void AddDuplicateSkillError(string field)
        {
            ModelState.AddModelError(field, "You can't have repeated skills");
        }

        public async Task<CandidateViewModel> GetCandidateAsync()
        {
            var id = GetIdFromRequest();
            var candidateViewModel = await service.GetCandidateViewModelByIdAsync(id.Value);
            return candidateViewModel;
        }
        public async Task<CandidateViewModel> GetCandidateModelAndBindWithStaticAsync(CandidateUserModel candidateModel)
        {
            var id = GetIdFromRequest();
            var candidateViewModel = await service.GetCandidateViewModelByIdAsync(candidateModel, id.Value);
            return candidateViewModel;
        }

        public async Task<CandidateUser> UpdateCandidate(CandidateUserModel model)
        {
            var id = GetIdFromRequest();
            return await service.UpdateCandidateUserAsync(model, id.Value);
        }
    }
}