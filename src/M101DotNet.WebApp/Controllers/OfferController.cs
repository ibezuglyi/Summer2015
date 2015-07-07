using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class OfferController : Controller
    {
        private JobContext dbContext;

        public OfferController()
        {
            dbContext = new JobContext();
        }


        public ActionResult Create()
        {  
            return View(new JobOffer());
        }

        [HttpPost]
        public async Task<ActionResult> Create(JobOffer model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Name = "dsadsa"; //temp
            model.IdRecruiter = "559ba3b789a9e61824b14730"; //temp
            await CreateJobOfferAsync(model);
            return RedirectToAction("Index", "Home");
        }

        public async Task CreateJobOfferAsync(JobOffer model)
        {
            var offer = new JobOffer
            {
                Name = model.Name,
                Salary = model.Salary,
                IdRecruiter = model.IdRecruiter
            };

            await dbContext.JobOffers.InsertOneAsync(offer);
        }
	}
}