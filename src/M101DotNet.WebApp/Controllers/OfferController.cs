using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Models.Offer;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class OfferController : Controller
    {
        private IApplicationService service;

        public OfferController(IApplicationService applicationService)
        {
            service = applicationService;
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (service.IsRecruiter(Request))
            {
                return View(new OfferModel());
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Create(OfferModel model)
        {
            if (ValidateForm(model))
            {
                await service.CreateJobOfferForRecruiter(model, Request);
                return RedirectToAction("OffersList", "Offer");
            }       
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> OffersList()
        {
            if (service.IsRecruiter(Request))
            {
                 var offers = await service.GetRecruiterOfferListViewModelAsync(Request);
                 return View(offers);               
            }
            return RedirectToAction("DeniedPermision", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Remove(string id)
        {
            var ifRecruiterHaveRightsToRemove = await GetOfferAndCheckOwnerAnOffer(id);
            if (service.IsAuthenticated(Request) && ifRecruiterHaveRightsToRemove)
            {
                await service.RemoveJobOfferAsync(id);
                return RedirectToAction("OffersList", "Offer");
            }
            return RedirectToAction("DeniedPermision", "Home");
        }        

        [HttpGet]
        public async Task<ActionResult> Details(string id)
        {
            if (service.IsAuthenticated(Request))
            {
                var offer = await service.GetOfferViewModelByIdAsync(id);
                return View(offer);
            }
            return RedirectToAction("DeniedPermision", "Home");
        }        

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (service.IsRecruiter(Request))
            {
                var ifRecruiterHaveRightsToEdit = await GetOfferAndCheckOwnerAnOffer(id);
                if (ifRecruiterHaveRightsToEdit)
                {
                    var offer = await service.GetOfferViewModelByIdAsync(id);    
                    return View(offer);
                }
                else
                {
                    return RedirectToAction("DeniedPermision", "Home");
                }
            }
            return RedirectToAction("DeniedPermision", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(OfferModel model)
        {
            if (ValidateForm(model))
            {
                await service.UpdateJobOfferAsync(model, model.Id);
                return RedirectToAction("OffersList", "Offer");
            }
            var offerViewModel = service.GetOfferViewModelAsync(model, Request);
            return View(offerViewModel);
        }

        private bool ValidateForm(OfferModel model)
        {
            if(ModelState.IsValid)
            {
                if (model.Skills.Count < 1)
                {
                    ModelState.AddModelError("notEnoughSkills", "Choose one or more skills");
                }
                if (service.AreSkillsDuplicated(model.Skills))
                {
                    ModelState.AddModelError("duplicateSkills", "You can't have repeated skills");
                }
            }            
            return ModelState.IsValid;
        }

        private async Task<bool> GetOfferAndCheckOwnerAnOffer(string offerId)
        {
            var idRecruiter = await GetIdRecruiterByOfferIdAsync(offerId);
            return service.IfCurrentUserAnOwnerOfOffer(idRecruiter, Request);
        }

        public async Task<string> GetIdRecruiterByOfferIdAsync(string offerId)
        {
            var offerViewModel = await service.GetOfferViewModelByIdAsync(offerId);
            return offerViewModel.IdRecruiter;
        }

	}
}