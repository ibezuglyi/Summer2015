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
        [Range(0, int.MaxValue, ErrorMessage = "Min salary should be [0 .. 2 147 483 647]")]
        public int? MinSalary { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Max salary should be [0 .. 2 147 483 647]")]
        public int? MaxSalary { get; set; }
        public List<SkillModel> Skills { get; set; }
        public bool IsSalarySort { get; set; }
        public bool IsScoreSort { get; set; }
        public bool IsDateSort { get; set; }

        public OfferSearchModel()
        {
            Skills = new List<SkillModel>();
        }

        public OfferSearchModel(List<SkillModel> skills, int? minSalary)
        {
            Skills = skills;
            MinSalary = minSalary;
        }

        public OfferSearchModel(List<SkillModel> skills, int? minSalary, int? maxSalary, string name, bool isSalarySort, bool isScoreSort, bool isDateSort)
        {   
            Skills = skills;
            MinSalary = minSalary;
            MaxSalary = maxSalary;
            Name = name;
            IsSalarySort = isSalarySort;
            IsScoreSort = isScoreSort;
            IsDateSort = isDateSort;
        }
    }
}