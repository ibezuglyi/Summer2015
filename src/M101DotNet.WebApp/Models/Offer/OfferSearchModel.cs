using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.Models.Candidate;

namespace WebApp.Models.Offer
{
    public class OfferSearchModel
    {
        public string Name { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Salary value can't be negative")]
        public int? MinSalary { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Salary value can't be negative")]
        public int? MaxSalary { get; set; }
        public List<SkillModel> Skills { get; set; }

        public OfferSearchModel()
        {
            Skills = new List<SkillModel>();
        }

        public OfferSearchModel(List<SkillModel> skills, int? minSalary, int? maxSalary, string name)
        {   
            Skills = skills;
            MinSalary = minSalary;
            MaxSalary = maxSalary;
            Name = name;

        }
    }
}