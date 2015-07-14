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
        private IApplicationService service;

        public OfferSearchController(IApplicationService applicationService)
        {
            service = applicationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var offerSearchViewModel = await service.GetDefaultOfferSearchViewModel(Request);
            return View(offerSearchViewModel);
        }

        [HttpPost]
        public ActionResult Index(OfferSearchController model)
        {
            return View();
        }
	}
}