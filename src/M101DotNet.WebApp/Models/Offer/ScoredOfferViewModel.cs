using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Candidate;

namespace WebApp.Models.Offer
{
    public class ScoredOfferViewModel
    {
        const int TopSkillNumber = 5;
        public ScoredOfferModel Offer { get; set; }
        public List<SkillModel> TopSkills { get; set; }

        public ScoredOfferViewModel()
        {
            Offer = new ScoredOfferModel();
            TopSkills = new List<SkillModel>();
        }
        public ScoredOfferViewModel(ScoredOfferModel offer)
        {
            Offer = offer;
            TopSkills = CalculateTopSkills(offer.Skills);
        }

        private List<SkillModel> CalculateTopSkills(List<SkillModel> skills)
        {            
            var sorterdSkills = skills.OrderByDescending(r => r.Level).ToList();
            var topSkills = sorterdSkills.Take(TopSkillNumber).ToList();
            return topSkills;
        }
    }
}