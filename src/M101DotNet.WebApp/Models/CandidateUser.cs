﻿using System.Collections.Generic;

namespace WebApp.Models
{
    public class CandidateUser : User
    {
        public string LastName { get; set; }
        public string ExperienceDescription { get; set; }
        public byte ExperienceInYears { get; set; }
        public int Salary { get; set; }
        public List<Skill> Skills { get; set; }


        public CandidateUser()
        {
            Skills = new List<Skill>();
        }

    }
}