using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace WebApp.Controllers
{
    public class RecruiterProfileController : Controller
    {
        //
        // GET: /RecruiterProfile/
        public async  Task<ActionResult> Index()
        {
            var recruiter = await GetRecruiter();
            return View(recruiter);
        }

        [HttpPost]
        public ActionResult Index(RecruiterUser model)
        {
            if (!ModelState.IsValid){
                return View();
            }
            var dbcontext = new JobContext();
            return View(model);
        }

        public async Task UpdateRecruiter()
        {
            var dbcontext = new JobContext();
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;
            var id = authManager.User.Claims.Single(r => r.Type == ClaimTypes.Sid);
        }

        public async Task<RecruiterUser> GetRecruiter()
        {
            var dbcontext = new JobContext();
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;
            var id = authManager.User.Claims.Single(r => r.Type == ClaimTypes.Sid);
            var recruiter = await dbcontext.RecruiterUsers.Find(r => r.Id == id.Value).SingleOrDefaultAsync();

            return recruiter;
        }


	}
}