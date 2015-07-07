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
    public class CandidateProfileController : UserProfileController
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
            if (IsAuthenticated())
            {
                var role = GetRoleFromRequest();
                if (role.Value == "Candidate")
                {
                    var candidate = await GetCandidateAsync();
                    candidate.Skills = new List<Skill>()
                    {
                        new Skill() {Level = 1, Name = "C#"},
                        new Skill() {Level = 2, Name = "PHP"}
                    };
                    return View(candidate);
                }
                }

                return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public async Task<ActionResult> Index(CandidateUser model)
        {            
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.ExperienceInYears < 0)
            {
                WrongSalaryExperienceError("experienceInYearsError");
                var candidate = await GetCandidateAsync();
                return View(candidate);
            }
            else if (model.Salary < 0)
            {
                WrongSalaryExperienceError("salaryError");
                var candidate = await GetCandidateAsync();
                return View(candidate);
            }
            
            var updatedModel = await UpdateCandidate(model);
            return View(updatedModel);
        }

        public void WrongSalaryExperienceError(string field)
        {
            ModelState.AddModelError(field, "It can't be a negative number");
        }

        public Task<CandidateUser> GetCandidateAsync()
        {
            var id = GetIdFromRequest();
            var candidate = service.GetCandidateByIdAsync(id.Value);
            return candidate;
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