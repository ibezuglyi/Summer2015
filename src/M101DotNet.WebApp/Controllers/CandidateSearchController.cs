using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.Candidate;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class CandidateSearchController : Controller
    {
        private IApplicationService _applicationService;
        private IAuthenticationService _authenticationService;

        public CandidateSearchController(IApplicationService applicationService, IAuthenticationService authenticationService)
        {
            _applicationService = applicationService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string id)
        {
            if (_authenticationService.IsRecruiter(Request))
            {
                if (id != null)
                {
                    var candidateSearchViewModel = await _applicationService.GetCandidateSearchViewModelForOffer(id);
                    return View(candidateSearchViewModel);
                }
            }
            return View(new CandidateSearchViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Index(CandidateSearchModel model)
        {
            if (ValidateForm(model))
            {
                var newCandidateSearchViewModel = await _applicationService.GetCandidatesSearchViewModelAsync(model);
                if (!newCandidateSearchViewModel.HasCandidates())
                {
                    AddNoCandidatesError();
                }
                return View(newCandidateSearchViewModel);
            }
            var offerSearchViewModelWithoutOffers = _applicationService.GetCandidatesSearchViewModelWithoutCandidates(model);
            return View(offerSearchViewModelWithoutOffers);
        }

        private bool ValidateForm(CandidateSearchModel model)
        {
            if (_applicationService.AreSkillsDuplicated(model.Skills))
            {
                ModelState.AddModelError("duplicateSkills", "You can't have repeated skills");
            }
            if (_applicationService.IsMinSalaryOverMaxSalary(model.MinSalary, model.MaxSalary))
            {
                ModelState.AddModelError("minOverMaxSalary", "Max salary must be grater or equal than min salary");
            }
            if (_applicationService.IsMinExperienceOverMaxExperience(model.MinExperienceInYears, model.MaxExperienceInYears))
            {
                ModelState.AddModelError("minOverMaxExperience", "Max experience must be greater or equal than min experience");
            }
            return ModelState.IsValid;
        }

        private void AddNoCandidatesError()
        {
            ModelState.AddModelError("noCandidates", "There are no candidates for given parameters");
        }
	}
}