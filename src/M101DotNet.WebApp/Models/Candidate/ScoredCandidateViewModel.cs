using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Candidate
{
    public class ScoredCandidateViewModel
    {
        const int TopSkillNumber = 5;
        public ScoredCandidateModel Candidate { get; set; }
        public string CandidateId { get; set; }
        public DateTime ModificationDate { get; set; }
        public List<SkillModel> TopSkills { get; set; }
        
        public ScoredCandidateViewModel()
        {
            Candidate = new ScoredCandidateModel();
            TopSkills = new List<SkillModel>();
        }

        public ScoredCandidateViewModel(ScoredCandidateModel candidate, DateTime modificationDate, string candidateId)
        {
            Candidate = candidate;
            CandidateId = candidateId;
            ModificationDate = modificationDate;
            TopSkills = CalculateTopSkills(candidate.Skills);
        }

        private List<SkillModel> CalculateTopSkills(List<SkillModel> skills)
        {
            var sorterdSkills = skills.OrderByDescending(r => r.Level).ToList();
            var topSkills = sorterdSkills.Take(TopSkillNumber).ToList();
            return topSkills;
        }
    }
}