using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.Offer;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class OfferSearchController : Controller
    {
        private IApplicationService _applicationService;
        private IAuthenticationService _authenticationService;

        public OfferSearchController(IApplicationService applicationService, IAuthenticationService authenticationService)
        {
            _applicationService = applicationService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var currentUserId = _authenticationService.GetUserIdFromRequest(Request);
            var offerSearchViewModel = await _applicationService.GetDefaultOfferSearchViewModel(currentUserId);
            return View(offerSearchViewModel);
        }
            return View(new OfferSearchViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Index(OfferSearchModel model)
        {
            if (ValidateForm(model))
            {
                var newOfferSearchViewModel = await service.GetOfferSearchViewModelAsync(model);
                return View(newOfferSearchViewModel);
            }
            var offerSearchViewModelWithoutOffers = service.GetOfferSearchViewModelWithoutOffersAsync(model);
            return View(offerSearchViewModelWithoutOffers);
        }


        private bool ValidateForm(OfferSearchModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Skills.Count < 1)
                {
                    ModelState.AddModelError("notEnoughSkills", "Choose one or more skills");
                }
                if (service.AreSkillsDuplicated(model.Skills))
                {
                    ModelState.AddModelError("duplicateSkills", "You can't have repeated skills");
                }
                if (service.IsMinSalaryOverMaxSalary(model.MinSalary, model.MaxSalary))
        {
                     ModelState.AddModelError("minOvermax", "MaxSalary must be grater or equal MinSalary");
                }
            }
            return ModelState.IsValid;
        }
	}
}