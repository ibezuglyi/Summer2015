using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Models.Candidate;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class CandidateProfileController : Controller
    {
        private IApplicationService _applicationService;
        private IAuthenticationService _authenticationService;

        public CandidateProfileController(IApplicationService applicationService, IAuthenticationService autheenticationService)
        {
            _applicationService = applicationService;
            _authenticationService = autheenticationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (_authenticationService.IsCandidate(Request))
            {
                var currentUserId = _authenticationService.GetUserIdFromRequest(Request);
                var candidateViewModel = await _applicationService.GetCandidateViewModelByIdAsync(currentUserId);
                AddNoSkillMessageIfNeeded(candidateViewModel.Candidate);
                return View(candidateViewModel);
            }
            return RedirectToAction("DeniedPermission", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string id)
        {
            var candidateViewModel = await _applicationService.GetCandidateViewModelByIdAsync(id);
            return View(candidateViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Index(CandidateUserModel model)
        {
            if (_authenticationService.IsCandidate(Request))
            {
                var currentUserId = _authenticationService.GetUserIdFromRequest(Request);
                if (ValidateForm(model))
                {
                    await _applicationService.UpdateCandidateUserAsync(model, currentUserId);
                    var updatedViewModel = await _applicationService.GetCandidateViewModelByIdAsync(currentUserId);
                    return View(updatedViewModel);
                }
                var viewModel = await _applicationService.GetCandidateViewModelByIdAsync(model, currentUserId);
                return View(viewModel);
            }
            return RedirectToAction("DeniedPermission", "Home");
        }

        private bool ValidateForm(CandidateUserModel model)
        {
            if (ModelState.IsValid) 
            {
                ValidateSkills(model.Skills);
            }
            return ModelState.IsValid;
        }

        public void AddNoSkillMessageIfNeeded(CandidateUserModel model)
        {
            if (!model.HasSkills())
            {
                ModelState.AddModelError("NoSkillMessage", "Add at least one skill to get access to 'Best offers'");
            }
        }

        public void ValidateSkills(List<SkillModel> skills)
        {
            if (skills.Count < 1)
            {
                ModelState.AddModelError("notEnoughSkills", "Choose one or more skills");
            }
            if (_applicationService.AreSkillsDuplicated(skills))
            {
                ModelState.AddModelError("duplicateSkills", "You can't have repeated skills");
            }
        }
    }
}