using System.Collections.Generic;

namespace WebApp.Models.Candidate
{
    public class CandidateUserModel
    {
        public int? ExperienceInYears { get; set; }
        public string ExperienceDescription { get; set; }
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