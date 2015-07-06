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
        public async Task<ActionResult> Index()
        {
            var user = await GetCandidate();
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

            Update();
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

        public async Task<CandidateUser> Update()
        {
            var dbcontext = new JobContext();
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;
            var id = authManager.User.Claims.Single(r => r.Type == ClaimTypes.Sid);
            var filter = Builders<CandidateUser>.Filter.Eq(r => r.Id, id.Value);
            var update = Builders<CandidateUser>
                .Update
                .Set(r => r.ExperienceDescription, "descryption")
                .Set(r => r.ExperienceInYears, 0)
                .Set(r => r.LastName, "lastname")
                .Set(r => r.Surname, "surname")
                .Set(r => r.Salary, 0);            

            await dbcontext.CandidateUsers.UpdateOneAsync(filter, update);
            return null;
        }
    }
}