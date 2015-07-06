using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class RecruiterUser : User
    {
        public string CompanyName { get; set; }

        public string CompanyDescription { get; set; }
    }
}