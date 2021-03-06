﻿using System;
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
            if (_authenticationService.IsCandidate(Request))
            {
                var currentUserId = _authenticationService.GetUserIdFromRequest(Request);
                var offerSearchViewModel = await _applicationService.GetDefaultOfferSearchViewModelAsync(currentUserId);
                return View(offerSearchViewModel);
            }
            return View(new OfferSearchViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Index(OfferSearchModel model)
        {
            if (ValidateForm(model))
            {
                var newOfferSearchViewModel = await _applicationService.GetOfferSearchViewModelAsync(model);
                if (!newOfferSearchViewModel.Offers.HasOffers())
                {
                    AddNoOffersError();
                }
                return View(newOfferSearchViewModel);
            }
            AddNoOffersError();
            var offerSearchViewModelWithoutOffers = _applicationService.GetOfferSearchViewModelWithoutOffers(model);
            return View(offerSearchViewModelWithoutOffers);
        }


        private bool ValidateForm(OfferSearchModel model)
        {
            if (ModelState.IsValid)
            {
                if (_applicationService.AreSkillsDuplicated(model.Skills))
                {
                    ModelState.AddModelError("duplicateSkills", "You can't have repeated skills");
                }
                if (_applicationService.IsMinSalaryOverMaxSalary(model.MinSalary, model.MaxSalary))
                {
                     ModelState.AddModelError("minOvermax", "Max salary must be grater or equal than min salary");
                }
            }
            return ModelState.IsValid;
        }

        private void AddNoOffersError()
        {
            ModelState.AddModelError("noOffers", "There are no offers for given parameters");
        }
	}
}