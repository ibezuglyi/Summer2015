using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Models.Offer;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class OfferController : Controller
    {
        private IApplicationService _applicationService;
        private IAuthenticationService _authenticationService;

        public OfferController(IApplicationService applicationService, IAuthenticationService authenticationService)
        {
            _applicationService = applicationService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (_authenticationService.IsRecruiter(Request))
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
                var userCreatingOfferId = _authenticationService.GetUserIdFromRequest(Request);
                await _applicationService.CreateJobOfferAsync(model, userCreatingOfferId);
                return RedirectToAction("OffersList", "Offer");
            }       
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> OffersList()
        {
            if (_authenticationService.IsRecruiter(Request))
            {
                var currentUserId = _authenticationService.GetUserIdFromRequest(Request);
                var offers = await _applicationService.GetOfferViewModelListAsync(currentUserId);
                return View(offers);               
            }
            return RedirectToAction("DeniedPermision", "Home");
        }

        [HttpGet]
        public ActionResult Remove()
        {
            return View();
        }     
   
        [HttpPost]
        public async Task<ActionResult> Remove(OfferModel model)
        {
            var ifRecruiterHaveRightsToRemove = await IsCurrentUserOwnerOfOffer(model.Id);
            if (_authenticationService.IsAuthenticated(Request) && ifRecruiterHaveRightsToRemove)
            {
                await _applicationService.RemoveJobOfferAsync(model.Id);
                return RedirectToAction("OffersList", "Offer");
            }
            return RedirectToAction("DeniedPermision", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Details(string id)
        {
            if (_authenticationService.IsAuthenticated(Request))
            {
                var offer = await _applicationService.GetOfferViewModelByIdAsync(id);
                return View(offer);
            }
            return RedirectToAction("DeniedPermision", "Home");
        }        

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (_authenticationService.IsRecruiter(Request))
            {
                var ifRecruiterHaveRightsToEdit = await IsCurrentUserOwnerOfOffer(id);
                if (ifRecruiterHaveRightsToEdit)
                {
                    var offer = await _applicationService.GetOfferViewModelByIdAsync(id);    
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
            var currentUserId = _authenticationService.GetUserIdFromRequest(Request);
            if (ValidateForm(model))
            {
                await _applicationService.UpdateJobOfferAsync(model, model.Id);
                return RedirectToAction("OffersList", "Offer");
            }
            var offerViewModel = _applicationService.GetOfferViewModelAsync(model, currentUserId);
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
                if (_applicationService.AreSkillsDuplicated(model.Skills))
                {
                    ModelState.AddModelError("duplicateSkills", "You can't have repeated skills");
                }
            }            
            return ModelState.IsValid;
        }

        private async Task<bool> IsCurrentUserOwnerOfOffer(string offerId)
        {
            var recruiterId = await _applicationService.GetIdRecruiterByOfferIdAsync(offerId);
            return IfCurrentUserAnOwnerOfOffer(recruiterId);
        }

        public bool IfCurrentUserAnOwnerOfOffer(string recruiterIdFromOffer)
        {
            var currentUserId = _authenticationService.GetUserIdFromRequest(Request);
            return currentUserId == recruiterIdFromOffer;
        }

	}
}