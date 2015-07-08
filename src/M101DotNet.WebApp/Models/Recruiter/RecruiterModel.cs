using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Recruiter
{
    public class RecruiterModel
    {
        public string CompanyName { get; set; }

        public string CompanyDescription { get; set; }
        public RecruiterModel() { }
        public RecruiterModel(string companyName, string companyDescription)
        {
            CompanyName = companyName;
            CompanyDescription = companyDescription;
        }
    }
}