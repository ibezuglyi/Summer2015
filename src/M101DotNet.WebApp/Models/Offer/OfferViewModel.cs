using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Candidate;

namespace WebApp.Models.Offer
{
    public class OfferViewModel
    {
        const int TopSkillNumber = 5;
        public OfferModel Offer { get; set; }
        public string IdRecruiter { get; set; }
        public List<SkillModel> TopSkills { get; set; }
        public DateTime ModificationDate { get; set; }

        public OfferViewModel()
        {
            Offer = new OfferModel();
            TopSkills = new List<SkillModel>();
        }
        public OfferViewModel(OfferModel offer, string idRecruiter, DateTime modificationDate)
        {
            Offer = offer;
            IdRecruiter = idRecruiter;
            ModificationDate = modificationDate;
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