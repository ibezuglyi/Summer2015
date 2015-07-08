using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
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

        public ActionResult Create()
        {
            if (IsAuthenticated())
            {
                var role = GetRoleFromRequest();
                if (role.Value == "Recruiter")
                {
                    return View(new JobOffer());
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Create(JobOffer model)
        {
            if (ValidateForm(model))
            {
                return View(model);
            }
                        
            model.IdRecruiter = GetIdRecruiterFromRequest().Value;
            
            //temporary solution
            
            model.Skills = new List<Skill>()
                    {
                        new Skill() {Level = 1, Name = "C#"},
                        new Skill() {Level = 2, Name = "PHP"},
                        new Skill() {Level = 2, Name = "Java"}
                    };

            await service.CreateJobOfferAsync(model);
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> OffersList()
        {
            if (IsAuthenticated())
            {
                var role = GetRoleFromRequest();
                if (role.Value == "Recruiter")
                {
                    var offers = await GetRecruiterOffersAsync();
                    return View(offers);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult OffersList(JobOffer model)
        {

            return View(model);
        }

        private Task<List<JobOffer>> GetRecruiterOffersAsync()
        {            
            var IdRecruiter = GetIdRecruiterFromRequest().Value;
            var offersRecruiter = service.GetOffersByIdRecruiterAsync(IdRecruiter);

            return offersRecruiter;
        }        

        private bool ValidateForm(JobOffer model)
        {
            bool isError = false;
            if(model.Salary<=0)
            {
                AddWrongSalaryValueError("salaryError");
                isError = true;
            }
            if(model.Name == null)
            {
                AddEmptyNameError("emptyName");
                isError = true;
            }
            return isError;
        }

        
        private void AddEmptyNameError(string field)
        {
            ModelState.AddModelError(field, "The Name can't be unfilled");
        }
        private void AddWrongSalaryValueError(string field)
        {
            ModelState.AddModelError(field, "The Salary must have a numeric not negative value");
        }

        

        private Claim GetIdRecruiterFromRequest()
        {
            var authManager = GetAuthManager();
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Sid);
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