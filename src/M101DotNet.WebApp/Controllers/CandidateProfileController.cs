using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using MongoDB.Driver;

namespace WebApp.Controllers
{
    public class CandidateProfileController : Controller
    {
        //
        // GET: /CandidateProfile/
        public ActionResult Index()
        {
            var user = GetCandidate();
            return View(user);
        }

        [HttpPost]
        public ActionResult Index(CandidateUser model)
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

            var dbContext = new JobContext();

            return View();
        }

        public void WrongSalaryExperienceError(string field)
        {
            ModelState.AddModelError(field, "It can't be a negative number");
        }

        public async Task<object> GetCandidate()
        {
            var dbcontext = new JobContext();
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;
            var id = authManager.User.Claims.Single(r => r.Type == ClaimTypes.Sid);
            var candidate = await dbcontext.CandidateUsers.Find(r => r.Id == id.Value).SingleOrDefaultAsync();
            return candidate;

        }
    }
}