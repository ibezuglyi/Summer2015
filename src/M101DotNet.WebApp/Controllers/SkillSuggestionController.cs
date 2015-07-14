using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class SkillSuggestionController : Controller
    {
        IApplicationService Service;

        public SkillSuggestionController(IApplicationService service)
        {
            Service = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetHints(string query)
        {
            List<string> hints = await Service.GetSortedSkillsMatchingQuery(query);
            var response = Service.MapToSkillSuggestionModel(query, hints);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
	}
}