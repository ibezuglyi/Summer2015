using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using MongoDB.Driver;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class CandidateProfileController : Controller
    {
        private IApplicationService service;

        public CandidateProfileController(IApplicationService applicationService)
        {
            service = applicationService;
        }
        //
        // GET: /CandidateProfile/
        public async Task<ActionResult> Index()
        {
            var candidate = await GetCandidate();
            return View(candidate);
        }

        [HttpPost]
        public async Task<ActionResult> Index(CandidateUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.Salary < 0)
            {
                WrongSalaryExperienceError("salary");
                return View(model);
            }
            if (model.ExperienceInYears < 0)
            {
                WrongSalaryExperienceError("experienceInYears");
                return View(model);
            }

            var updatedModel = await UpdateCandidate(model);
            return View(updatedModel);
        }

        public void WrongSalaryExperienceError(string field)
        {
            ModelState.AddModelError(field, "It can't be a negative number");
        }

        public Task<CandidateUser> GetCandidate()
        {
            var id = GetIdFromRequest();
            return service.GetCandidateByIdAsync(id.Value);
        }

        public Claim GetIdFromRequest()
        {
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;            
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Sid);
        }

        
        public async Task<CandidateUser> UpdateCandidate(CandidateUser model)
        {
            var id = GetIdFromRequest();
            return await service.UpdateCandidateUserAsync(model, id.Value);
        }
    }
}