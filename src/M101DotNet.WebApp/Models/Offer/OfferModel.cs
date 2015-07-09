using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Entities;
using WebApp.Models.Candidate;

namespace WebApp.Models.Offer
{
    public class OfferModel
    {
        public string Name { get; set; }
        public int Salary { get; set; }
        public List<SkillModel> Skills { get; set; }

        public OfferModel()
        {
            Skills = new List<SkillModel>();
        }

        public OfferModel(string name, int salary, List<SkillModel> skills)
        {
            Name = name;
            Salary = salary;
            Skills = skills;
        }
    }
}