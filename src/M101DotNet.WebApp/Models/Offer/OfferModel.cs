using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Entities;

namespace WebApp.Models.Offer
{
    public class OfferModel
    {
        public string Name { get; set; }
        public int Salary { get; set; }
        public List<Skill> Skills { get; set; }

        public OfferModel()
        {
            Skills = new List<Skill>();
        }

        public OfferModel(string name, int salary, List<Skill> skills)
        {
            Name = name;
            Salary = salary;
            Skills = skills;
        }
    }
}