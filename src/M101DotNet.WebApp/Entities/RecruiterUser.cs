using WebApp.Models;

namespace WebApp.Entities
{
    public class RecruiterUser : User
    {
        public string CompanyName { get; set; }

        public string CompanyDescription { get; set; }
    }
}