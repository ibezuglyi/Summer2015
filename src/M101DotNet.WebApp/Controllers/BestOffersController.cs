using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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
                var scoredOfferListViewModel = await _applicationService.GetOffersSortedByMatch(candidateModel);
                return View(scoredOfferListViewModel);
            }
            return RedirectToAction("DeniedPermission", "Home");
        }
	}
}