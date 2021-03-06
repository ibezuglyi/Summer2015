﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.Entities;
using WebApp.Models.Candidate;

namespace WebApp.Models.Offer
{
    public class OfferModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Field name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field salary is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Salary should be [0 .. 2 147 483 647]")]
        public int Salary { get; set; }

        [Required(ErrorMessage = "Field Description is required")]
        public string Description { get; set; }
        public List<SkillModel> Skills { get; set; }

        public OfferModel()
        {
            Skills = new List<SkillModel>();
        }

        public OfferModel(string offerId, string name, int salary, string description, List<SkillModel> skills)
        {
            Id = offerId;
            Name = name;
            Salary = salary;
            Description = description;
            Skills = skills;
        }
    }
}