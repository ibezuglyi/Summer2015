using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.Entities
{
    public class CandidateUser : User
    {
        public string ExperienceDescription { get; set; }

        [Range(0, 100, ErrorMessage="Experience must be between 0 and 100 years")]
        public int? ExperienceInYears { get; set; }

        [Range(0, int.MaxValue, ErrorMessage="Salary value can't be negative")]
        public int? Salary { get; set; }
        public List<Skill> Skills { get; set; }


        public CandidateUser()
        {
            Skills = new List<Skill>();
        }

    }
}