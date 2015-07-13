using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
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
            if (IsAuthenticated() && IsRecruiter())
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
                var idRecruiter = GetIdRecruiterFromRequest().Value;
                await service.CreateJobOfferAsync(model, idRecruiter);
                return RedirectToAction("OffersList", "Offer");
            }       
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> OffersList()
        {
            if (IsRecruiter())
            {
                 var offers = await GetRecruiterOffersAsync();
                 return View(offers);               
            }
            return RedirectToAction("DeniedPermision", "Home");
        }
                
        [HttpGet]
        public async Task<ActionResult> Details(string id)
        {
            if (IsAuthenticated())
            {
                var offer = await GetOfferViewModelAsync(id);
                return View(offer);
            }
            return RedirectToAction("DeniedPermision", "Home");
        }        

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {            
            if (IsAuthenticated() && IsRecruiter())
            {
                var offer = await GetOfferViewModelAsync(id);
                var isRightRecruiter = IfCurrentUserAnOwnerOfOffer(offer.IdRecruiter);
                if (isRightRecruiter)
                {
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
            //Not finished yet
            if (ValidateForm(model))
            {
                await UpdateOffer(model, model.Id);
            }
            return View(model);
        }

        public async Task UpdateOffer(OfferModel model, string idOffer)
        {
            await service.UpdateJobOfferAsync(model, idOffer);
        }

        private Task<OfferListViewModel> GetRecruiterOffersAsync()
        {            
            var recruiterId = GetIdRecruiterFromRequest().Value;
            var offersRecruiter = service.GetOfferViewModelListAsync(recruiterId);

            return offersRecruiter;
        }


        private bool ValidateForm(OfferModel model)
        {
            if (model.Skills.Count < 1)
            {
                ModelState.AddModelError("notEnoughSkills", "Choose one or more skills");
            }
            if (AreSkillsDuplicate(model))
            {
                ModelState.AddModelError("duplicateSkills", "You can't have repeated skills");
            }
            return ModelState.IsValid;
        }

        private bool AreSkillsDuplicate(OfferModel model)
        {
            var skills = model.Skills;
            var skillsDistinct = model.Skills.Select(r => r.Name).Distinct();
            return skills.Count != skillsDistinct.Count();
        }

       
        private bool IfCurrentUserAnOwnerOfOffer(string idRecruiter)
        {
            var id = GetIdRecruiterFromRequest().Value;
            return (id == idRecruiter);            
        }

        private bool IsRecruiter()
        {
            var role = GetRoleFromRequest();
            return (role.Value == "Recruiter");
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

        private Claim GetIdRecruiterFromRequest()
        {
            var authManager = GetAuthManager();
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Sid);
        }

        public Task<OfferViewModel> GetOfferViewModelAsync(string offerId)
        {
            var offerModel = service.GetOfferViewModelByIdAsync(offerId);
            return offerModel;
        }

        //I know that methods below are opposite to DRY
        
        
        

        private Claim GetRoleFromRequest()
        {
            var authManager = GetAuthManager();
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Role);
        }

        private bool IsAuthenticated()
        {
            var authManager = GetAuthManager();
            return authManager.User.Identity.IsAuthenticated;
        }

        private Microsoft.Owin.Security.IAuthenticationManager GetAuthManager()
        {
            var context = Request.GetOwinContext();
            return context.Authentication;
        }
	}
}