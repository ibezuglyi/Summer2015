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
    public class RecruiterProfileController : Controller
    {
        private IApplicationService service;

        public RecruiterProfileController(IApplicationService applicationService)
        {
            service = applicationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (service.IsRecruiter(Request))
            {
                var recruiterViewModel = await service.GetRecruiterViewModelAsync(Request);
                return View(recruiterViewModel);                
            }
            return RedirectToAction("DeniedPermision", "Home");          
        }

        [HttpPost]
        public async Task<ActionResult> Index(RecruiterModel model)
        {    
            await service.UpdateRecruiter(model, Request);
            return RedirectToAction("Index", "Home");
        }
	}
}