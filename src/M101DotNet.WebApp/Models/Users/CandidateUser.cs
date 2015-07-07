using System.Collections.Generic;

namespace WebApp.Models
{
    public class CandidateUser : User
    {
        public string ExperienceDescription { get; set; }
        public int ExperienceInYears { get; set; }
        public int Salary { get; set; }
        public List<Skill> Skills { get; set; }


        public CandidateUser()
        {
            Skills = new List<Skill>();
        }

    }
}