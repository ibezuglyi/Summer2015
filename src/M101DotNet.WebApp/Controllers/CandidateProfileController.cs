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
            var user = await GetCandidate();
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> Index(CandidateUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if(model.Salary < 0)
            {
                WrongSalaryExperienceError("Salary");
                return View(model);
            }
            if (model.ExperienceInYears < 0)
            {
                WrongSalaryExperienceError("ExperienceInYears");
                return View(model);
            }

            await UpdateCandidate(model);
            return View();
        }

        public void WrongSalaryExperienceError(string field)
        {
            ModelState.AddModelError(field, "It can't be a negative number");
        }

        public async Task<CandidateUser> GetCandidate()
        {
            var id = GetIdFromRequest();
            return await service.GetCandidateByIdAsync(id.Value);
        }

        public Claim GetIdFromRequest()
        {
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;            
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Sid);
        }

        
        public async Task UpdateCandidate(CandidateUser model)
        {
            var id = GetIdFromRequest();
            await service.UpdateCandidateUserAsync(model, id.Value);
        }
    }
}