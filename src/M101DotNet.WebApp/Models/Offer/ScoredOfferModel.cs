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
        public string Name { get; set; }
        public int Salary { get; set; }
        public double Score {get; set;}
        public string Description { get; set; }
        public List<SkillModel> Skills { get; set; }

        public ScoredOfferModel()
        {
            Skills = new List<SkillModel>();
        }

        public ScoredOfferModel(string offerId, string name, int salary, double score, string description, List<SkillModel> skills)
        {
            Id = offerId;
            Name = name;
            Salary = salary;
            Score = score;
            Description = description;
            Skills = skills;
        }
    }
}