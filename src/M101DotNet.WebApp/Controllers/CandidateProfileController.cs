﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class CandidateProfileController : UserProfileController
    {
        private IApplicationService service;

        public CandidateProfileController(IApplicationService applicationService)
        {
            service = applicationService;
        }
        //
        // GET: /CandidateProfile/
        public async Task<ActionResult> Index()
        {
            if (IsAuthenticated())
            {
                var role = GetRoleFromRequest();
                if (role.Value == "Candidate")
                {
                    var candidate = await GetCandidateAsync();
                    candidate.Skills = new List<Skill>()
                    {
                        new Skill() {Level = 1, Name = "C#"},
                        new Skill() {Level = 2, Name = "PHP"}
                    };
                    return View(candidate);
                }
                }

                return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public async Task<ActionResult> Index(CandidateUser model)
        {            
            if (ValidateForm(model))
            {
                var updatedModel = await UpdateCandidate(model);
                return View(updatedModel);
            }
            else
            {
                var oldModel = await GetCandidateAsync();
                return View(oldModel);
            }

        }

        private bool ValidateForm(CandidateUser model)
        {
            if (!ModelState.IsValid)
            {
                AddWrongFieldValueError("wrongFieldValue");
            }
            if (model.ExperienceInYears < 0)
            {
                WrongSalaryExperienceError("experienceInYearsError");
            }
            if (model.Salary < 0)
            {
                WrongSalaryExperienceError("salaryError");
            }
            for (int i = 0; i < model.Skills.Count; i++)
            {
                for (int j = i; j < model.Skills.Count; j++)
                {
                    if (i != j && model.Skills[i].Name == model.Skills[j].Name)
                    {
                        AddDuplicateSkillError("duplicateSkills");
                    }
                }
                if (model.Skills[i].Level < 1 || model.Skills[i].Level > 10)
                {
                    AddWrongLevelError("wrongLevel");
                }
            }
            return ModelState.IsValid;
        }

        private void AddWrongFieldValueError(string field)
        {
            ModelState.AddModelError(field, "One of the fields has wrong value");
        }

        private void AddWrongLevelError(string field)
        {
            ModelState.AddModelError(field, "Wrong level");
            }
            
        public void AddDuplicateSkillError(string field)
        {
            ModelState.AddModelError(field, "You can't have repeated skills");
        }

        public void AddWrongSalaryExperienceError(string field)
        {
            ModelState.AddModelError(field, "It can't be a negative number");
        }

        public Task<CandidateUser> GetCandidateAsync()
        {
            var id = GetIdFromRequest();
            var candidate = service.GetCandidateByIdAsync(id.Value);
            return candidate;
        }



        
        public async Task<CandidateUser> UpdateCandidate(CandidateUser model)
        {
            var id = GetIdFromRequest();
            return await service.UpdateCandidateUserAsync(model, id.Value);
        }
    }
}