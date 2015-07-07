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
    public class RecruiterProfileController : UserProfileController
    {
        private IApplicationService service;

        public RecruiterProfileController(IApplicationService applicationService)
        {
            service = applicationService;
        }

        public async Task<ActionResult> Index()
        {
            if (IsAuthenticated())
            {
                var role = GetRoleFromRequest();
                if (role.Value == "Recruiter")
                {
                    var recruiter = await GetRecruiterAsync();
                    return View(recruiter);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> Index(RecruiterUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var updatedModel = await UpdateRecruiter(model);
            return View(updatedModel);
        }

        public async Task<RecruiterUser> UpdateRecruiter(RecruiterUser model)
        {
            var id = GetIdFromRequest();
            return await service.UpdateRecruiterUserAsync(model, id.Value);
        }

        public Task<RecruiterUser> GetRecruiterAsync()
        {
            var id = GetIdFromRequest();
            return service.GetRecruiterByIdAsync(id.Value);
        }

        
	}
}