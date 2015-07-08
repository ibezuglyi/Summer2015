using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Recruiter
{
    public class RecruiterViewModel
    {
        
        public RecruiterModel Recruiter { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public RecruiterViewModel(RecruiterModel recruiter, string name, string email)
        {
            Recruiter = recruiter;
            Name = name;
            Email = email;
        }

        
    }
}