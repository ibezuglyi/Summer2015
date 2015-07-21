using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Candidate
{
    public class ScoredCandidateModel
    {
        public int? ExperienceInYears { get; set; }
        public string ExperienceDescription { get; set; }
        public int? Salary { get; set; }
        public double Score {get; set; }
        public string Name { get; set; }
        public List<SkillModel> Skills { get; set; }

        public ScoredCandidateModel()
        {
            Skills = new List<SkillModel>();
        }

        public ScoredCandidateModel(string name, int? salary, string experienceDescription, int? experienceInYears, double score,  List<SkillModel> skillModels)
        {
            Name = name;
            ExperienceInYears = experienceInYears;
            ExperienceDescription = experienceDescription;
            Salary = salary;
            Score = score;
            Skills = skillModels;
        }
    }
}