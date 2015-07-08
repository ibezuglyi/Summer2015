using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Entities;
using WebApp.Models;
using WebApp.Models.Recruiter;
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
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (IsAuthenticated() && IsRecruiter())
            {
                var recruiterViewModel = await GetRecruiterAsync();
                return View(recruiterViewModel);                
            }
            else
            {
                return RedirectToAction("DeniedPermision", "Home");
            }            
        }

        [HttpPost]
        public async Task<ActionResult> Index(RecruiterModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = GetRecruiterAsync();
                return View(viewModel);
            }            
            await UpdateRecruiter(model);
            return RedirectToAction("Index", "Home");
        }

        public bool IsRecruiter()
        {
            var role = GetRoleFromRequest();
            return (role.Value == "Recruiter") ? true : false;
        }

        public async Task UpdateRecruiter(RecruiterModel model)
        {
            var id = GetIdFromRequest();
            await service.UpdateRecruiterModelAsync(model, id.Value);
        }

        public Task<RecruiterViewModel> GetRecruiterAsync()
        {
            var id = GetIdFromRequest();
            var recruiterModel = service.GetRecruiterViewModelByIdAsync(id.Value);
            return recruiterModel;
        }

        
	}
}