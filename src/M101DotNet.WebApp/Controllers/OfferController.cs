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
        private readonly IAuthenticationService _authenticationService;

        public OfferController(IApplicationService applicationService, IAuthenticationService authenticationService)
        {
            service = applicationService;
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
                var recruiterId = _authenticationService.GetRecruiterIdFromRequest(Request);
                await service.CreateJobOfferAsync(model, recruiterId);
                return RedirectToAction("OffersList", "Offer");
            }       
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> OffersList()
        {
            if (_authenticationService.IsRecruiter(Request))
            {
                 var offers = await GetRecruiterOfferListViewModelAsync();
                 return View(offers);               
            }
            return RedirectToAction("DeniedPermision", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Remove(string id)
        {
            var ifRecruiterHaveRightsToRemove = await GetOfferAndCheckOwnerAnOffer(id);
            if (_authenticationService.IsAuthenticated(Request) && ifRecruiterHaveRightsToRemove)
            {
                await RemoveOffer(id);
                return RedirectToAction("OffersList", "Offer");
            }
            return RedirectToAction("DeniedPermision", "Home");
        }        

        [HttpGet]
        public async Task<ActionResult> Details(string id)
        {
            if (_authenticationService.IsAuthenticated(Request))
            {
                var offer = await GetOfferViewModelAsync(id);
                return View(offer);
            }
            return RedirectToAction("DeniedPermision", "Home");
        }        

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (_authenticationService.IsRecruiter(Request))
            {
                var ifRecruiterHaveRightsToEdit = await GetOfferAndCheckOwnerAnOffer(id);

                if (ifRecruiterHaveRightsToEdit)
                {
                    var offer = await GetOfferViewModelAsync(id);    
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
                await UpdateOffer(model, model.Id);
                return RedirectToAction("OffersList", "Offer");
            }
            var offer = GetOfferViewModel(model);
            return View(offer);
        }

        

        public async Task UpdateOffer(OfferModel model, string idOffer)
        {
            await service.UpdateJobOfferAsync(model, idOffer);
        }

        public async Task RemoveOffer(string idOffer)
        {
            await service.RemoveJobOfferAsync(idOffer);
        }

        private Task<OfferListViewModel> GetRecruiterOfferListViewModelAsync()
        {
            var recruiterId = _authenticationService.GetRecruiterIdFromRequest(Request);
            var offersRecruiter = service.GetOfferViewModelListAsync(recruiterId);
            return offersRecruiter;
        }


        private bool ValidateForm(OfferModel model)
        {
            if(ModelState.IsValid)
            {
                if (model.Skills.Count < 1)
                {
                    ModelState.AddModelError("notEnoughSkills", "Choose one or more skills");
                }
                if (AreSkillsDuplicate(model))
                {
                    ModelState.AddModelError("duplicateSkills", "You can't have repeated skills");
                }
            }            
            return ModelState.IsValid;
        }

        private bool AreSkillsDuplicate(OfferModel model)
        {
            var skills = model.Skills;
            var skillsDistinct = model.Skills.Select(r => r.Name.ToLower()).Distinct();
            return skills.Count != skillsDistinct.Count();
        }

        private async Task<bool> GetOfferAndCheckOwnerAnOffer(string offerId)
        {
            var idRecruiter = await GetIdRecruiterByOfferIdAsync(offerId);
            return IfCurrentUserAnOwnerOfOffer(idRecruiter);
        }
       
        private bool IfCurrentUserAnOwnerOfOffer(string recruiterId)
        {
            var id = _authenticationService.GetRecruiterIdFromRequest(Request);
            return id == recruiterId;            
        }

        

        private void AddEmptyNameError(string field)
        {
            ModelState.AddModelError(field, "The Name can't be unfilled");
        }
        private void AddWrongSalaryValueError(string field)
        {
            ModelState.AddModelError(field, "The Salary must have a numeric not negative value");
        }

        public async Task<string> GetIdRecruiterByOfferIdAsync(string offerId)
        {
            var offer = await GetOfferViewModelAsync(offerId);
            return offer.IdRecruiter;
        }

        

        public Task<OfferViewModel> GetOfferViewModelAsync(string offerId)
        {
            var offerModel = service.GetOfferViewModelByIdAsync(offerId);
            return offerModel;
        }

        public OfferViewModel GetOfferViewModel(OfferModel offerModel)
        {
            var recruiterId = _authenticationService.GetRecruiterIdFromRequest(Request);
            var offerViewModel = new OfferViewModel(offerModel, recruiterId);
            return offerViewModel;
        }

	}
}