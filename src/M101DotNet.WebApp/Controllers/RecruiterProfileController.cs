using MongoDB.Driver;
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
    public class RecruiterProfileController : Controller
    {
        private IApplicationService service;

        public RecruiterProfileController(IApplicationService applicationService)
        {
            service = applicationService;
        }

        public async Task<ActionResult> Index()
        {
            var recruiter = await GetRecruiter();
            return View(recruiter);
        }

        [HttpPost]
        public async Task<ActionResult> Index(RecruiterUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await UpdateRecruiter(model);
            return View(model);
        }

        public async Task UpdateRecruiter(RecruiterUser model)
        {
            var id = GetIdFromRequest();
            await service.UpdateRecruiterUserAsync(model, id.Value);
        }

        public Task<RecruiterUser> GetRecruiter()
        {
            var id = GetIdFromRequest();
            return service.GetRecruiterByIdAsync(id.Value);
        }

        public Claim GetIdFromRequest()
        {
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Sid);
        }
      
	}
}