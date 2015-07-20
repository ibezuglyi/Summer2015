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
        private IApplicationService _applicationService;
        private IAuthenticationService _authenticationService;

        public RecruiterProfileController(IApplicationService applicationService, IAuthenticationService authenticationService)
        {
            _applicationService = applicationService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (_authenticationService.IsRecruiter(Request))
            {
                var currentUserId = _authenticationService.GetUserIdFromRequest(Request);
                var recruiterViewModel = await _applicationService.GetRecruiterViewModelByIdAsync(currentUserId);
                return View(recruiterViewModel);                
            }
            return RedirectToAction("DeniedPermission", "Home");          
        }

        [HttpPost]
        public async Task<ActionResult> Index(RecruiterModel model)
        {
            var currentUserId = _authenticationService.GetUserIdFromRequest(Request);
            await _applicationService.UpdateRecruiterModelAsync(model, currentUserId);
            return RedirectToAction("Index", "Home");
        }
	}
}