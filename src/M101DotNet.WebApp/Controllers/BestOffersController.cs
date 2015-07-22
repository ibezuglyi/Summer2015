using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.Candidate;
using WebApp.Models.Offer;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class BestOffersController : Controller
    {
        private IApplicationService _applicationService;
        private IAuthenticationService _authenticationService;

        public BestOffersController(IApplicationService applicationService, IAuthenticationService authenticationService)
        {
            _applicationService = applicationService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public async Task <ActionResult> Index()
        {
            if (_authenticationService.IsCandidate(Request))
            {
                var candidateId = _authenticationService.GetUserIdFromRequest(Request);
                var candidateModel = await _applicationService.GetCandidateUserModelByIdAsync(candidateId);
                if (candidateModel.HasSkills())
                {
                    var scoredOfferListViewModel = await _applicationService.GetOffersSortedByScoreAsync(candidateModel);
                    return View(scoredOfferListViewModel);                    
                }
                AddNoSkillsError();
                return View(new ScoredOfferListViewModel());
            }
            return RedirectToAction("DeniedPermission", "Home");
        }

        public void AddNoSkillsError()
        {
            ModelState.AddModelError("FillSkillsMessage", "There are no offers becouse lack of skills in your profile. Edit your profile and add at least one skill.");
        }
	}
}