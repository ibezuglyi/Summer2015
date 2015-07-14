using System;
using System.Collections.Generic;
using System.Linq;
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
        public JsonResult GetHints(string query)
        {
            List<string> hints = Service.GetSkillsMatchingQuery(query);

            var response = new SkillSuggestionModel(query, hints);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
	}
}