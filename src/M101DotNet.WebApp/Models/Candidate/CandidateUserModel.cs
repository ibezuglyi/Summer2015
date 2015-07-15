using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Candidate
{
    public class CandidateUserModel
    {
        [Range(0, 100, ErrorMessage = "Experience must be between 0 and 100 years")]
        public int? ExperienceInYears { get; set; }

        public string ExperienceDescription { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Salary should be [0 .. 2 147 483 647]")]
        public int? Salary { get; set; }

        public List<SkillModel> Skills { get; set; }

        public CandidateUserModel()
        {
            Skills = new List<SkillModel>();
        }

        public CandidateUserModel(int? salary, string experienceDescription, int? experienceInYears, List<SkillModel> skillModels)
        {
            ExperienceInYears = experienceInYears;
            ExperienceDescription = experienceDescription;
            Salary = salary;
            Skills = skillModels;
        }
    }
}