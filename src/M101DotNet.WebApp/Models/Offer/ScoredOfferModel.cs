using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.Models.Candidate;

namespace WebApp.Models.Offer
{
    public class ScoredOfferModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Field name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field salary is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Salary value can't be negative")]
        public int Salary { get; set; }

        public double Score {get; set;}

        public List<SkillModel> Skills { get; set; }

        public ScoredOfferModel()
        {
            Skills = new List<SkillModel>();
        }

        public ScoredOfferModel(string offerId, string name, int salary, int score, List<SkillModel> skills)
        {
            Id = offerId;
            Name = name;
            Salary = salary;
            Score = score;
            Skills = skills;
        }
    }
}