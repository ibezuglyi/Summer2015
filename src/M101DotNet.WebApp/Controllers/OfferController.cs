using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class OfferController : Controller
    {
        private JobContext dbContext;

        public OfferController()
        {
            dbContext = new JobContext();
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
                        
            model.IdRecruiter = GetIdFromRequest().Value;
            await CreateJobOfferAsync(model);
            return RedirectToAction("Index", "Home");
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

        public async Task CreateJobOfferAsync(JobOffer model)
        {
            var offer = new JobOffer
            {
                Name = model.Name,
                Salary = model.Salary,
                IdRecruiter = model.IdRecruiter
            };

            await dbContext.JobOffers.InsertOneAsync(offer);
        }

        public Claim GetIdFromRequest()
        {
            var authManager = getAuthManager();
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Sid);
        }

        //I know that methods below are opposite to DRY
        public Claim GetRoleFromRequest()
        {
            var authManager = getAuthManager();
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Role);
        }

        public bool IsAuthenticated()
        {
            var authManager = getAuthManager();
            return authManager.User.Identity.IsAuthenticated;
        }

        public Microsoft.Owin.Security.IAuthenticationManager getAuthManager()
        {
            var context = Request.GetOwinContext();
            return context.Authentication;
        }
	}
}