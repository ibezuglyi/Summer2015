using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.Candidate
{
    public class CandidateSearchModel
    {
        [Range(0, int.MaxValue, ErrorMessage = "Min salary should be [0 .. 2 147 483 647]")]
        public int? MinSalary { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Max salary should be [0 .. 2 147 483 647]")]
        public int? MaxSalary { get; set; }

        public List<SkillModel> Skills { get; set; }

        public SortBy SortBy { get; set; }

        [Range(0, 100, ErrorMessage = "Min experience must be between 0 and 100 years")]
        public int? MinExperienceInYears {get; set;}

        [Range(0, 100, ErrorMessage = "Max experience must be between 0 and 100 years")]
        public int? MaxExperienceInYears { get; set; }

        public CandidateSearchModel()
        {
            Skills = new List<SkillModel>();
        }

    }
}