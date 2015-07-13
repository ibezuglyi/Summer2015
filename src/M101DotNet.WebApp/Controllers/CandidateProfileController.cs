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
        private IApplicationService service;

        public CandidateProfileController(IApplicationService applicationService)
        {
            service = applicationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (service.IsCandidate(Request))
            {
                var candidateViewModel = await service.GetCandidateViewModelAsync(Request);
                return View(candidateViewModel);
            }
            return RedirectToAction("DeniedPermision", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Detail(string id)
        {
            if (service.IsRecruiter(Request))
            {
                var candidateViewModel = await service.GetCandidateViewModelByIdAsync(id);
                return View(candidateViewModel);
            }
            return RedirectToAction("DeniedPermision", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Index(CandidateUserModel model)
        {
            if (service.IsCandidate(Request))
            {
                if (ValidateForm(model))
                {
                    await service.UpdateCandidate(model, Request);
                    var updatedViewModel = await service.GetCandidateViewModelAsync(Request);
                    return View(updatedViewModel);
                }
                var viewModel = await service.GetCandidateModelAndBindWithStaticAsync(model, Request);
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

        public void ValidateSkills(List<SkillModel> skills)
        {
            if (skills.Count < 1)
            {
                ModelState.AddModelError("notEnoughSkills", "Choose one or more skills");
            }
            if (service.AreSkillsDuplicated(skills))
            {
                ModelState.AddModelError("duplicateSkills", "You can't have repeated skills");
            }
        }
    }
}