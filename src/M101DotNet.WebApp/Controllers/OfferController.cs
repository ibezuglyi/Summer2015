﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Entities;
using WebApp.Models;
using WebApp.Models.Candidate;
using WebApp.Models.Offer;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class OfferController : Controller
    {
        private IApplicationService service;

        public OfferController(IApplicationService applicationService)
        {
            service = applicationService;
        }

        public ActionResult Create()
        {
            if (IsAuthenticated() && IsRecruiter())
            {
                return View(new OfferViewModel());
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Create(OfferModel model)
        {
            if (ValidateForm(model))
            {
                return View(model);
            }         
            //temporary solution            
            model.Skills = new List<SkillModel>()
                    {
                        new SkillModel() {Level = 1, Name = "C#"},
                        new SkillModel() {Level = 2, Name = "PHP"},
                        new SkillModel() {Level = 9, Name = "Java"},
                        new SkillModel() {Level = 4, Name = "C++"},
                        new SkillModel() {Level = 5, Name = "Java Script"},
                        new SkillModel() {Level = 3, Name = "Pyton"}
                    };

            var idRecruiter = GetIdRecruiterFromRequest().Value;
            await service.CreateJobOfferAsync(model, idRecruiter);
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> OffersList()
        {
            if (IsAuthenticated() && IsRecruiter())
            {
                 var offers = await GetRecruiterOffersAsync();
                 return View(offers);               
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult OffersList(JobOffer model)
        {

            return View(model);
        }

        private Task<OfferViewModelList> GetRecruiterOffersAsync()
        {            
            var IdRecruiter = GetIdRecruiterFromRequest().Value;
            var offersRecruiter = service.GetOfferViewModelListAsync(IdRecruiter);

            return offersRecruiter;
        }        

        private bool ValidateForm(OfferModel model)
        {
            bool isError = false;
            if(model.Salary<=0)
            {
                AddWrongSalaryValueError("salaryError");
                isError = true;
            }
            if(model.Name == null)
            {
                AddEmptyNameError("emptyName");
                isError = true;
            }
            return isError;
        }

        private bool IsRecruiter()
        {
            var role = GetRoleFromRequest();
            return (role.Value == "Recruiter");
        }

        private void AddEmptyNameError(string field)
        {
            ModelState.AddModelError(field, "The Name can't be unfilled");
        }
        private void AddWrongSalaryValueError(string field)
        {
            ModelState.AddModelError(field, "The Salary must have a numeric not negative value");
        }

        

        private Claim GetIdRecruiterFromRequest()
        {
            var authManager = GetAuthManager();
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Sid);
        }

        public Task<OfferViewModel> GetOfferAsync(string offerId)
        {
            var offerModel = service.GetOfferViewModelByIdAsync(offerId);
            return offerModel;
        }

        //I know that methods below are opposite to DRY
        
        
        

        private Claim GetRoleFromRequest()
        {
            var authManager = GetAuthManager();
            return authManager.User.Claims.Single(r => r.Type == ClaimTypes.Role);
        }

        private bool IsAuthenticated()
        {
            var authManager = GetAuthManager();
            return authManager.User.Identity.IsAuthenticated;
        }

        private Microsoft.Owin.Security.IAuthenticationManager GetAuthManager()
        {
            var context = Request.GetOwinContext();
            return context.Authentication;
        }
	}
}