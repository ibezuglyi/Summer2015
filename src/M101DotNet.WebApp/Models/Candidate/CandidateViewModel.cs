using System;
namespace WebApp.Models.Candidate
{
    public class CandidateViewModel
    {
        public CandidateUserModel Candidate { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime ModificationDate { get; set; }

        public CandidateViewModel(CandidateUserModel candidate, string name, string email, DateTime modificationDate)
        {
            Candidate = candidate;
            Name = name;
            Email = email;
            ModificationDate = modificationDate.ToLocalTime();
        }
    }
}