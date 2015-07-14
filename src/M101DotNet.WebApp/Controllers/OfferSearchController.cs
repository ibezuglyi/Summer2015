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

        [HttpPost]
        public ActionResult Index(OfferSearchController model)
        {
            return View();
        }
	}
}