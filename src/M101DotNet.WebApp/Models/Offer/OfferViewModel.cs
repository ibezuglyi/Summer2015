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

        public OfferViewModel()
        {
            Offer = new OfferModel();
            TopSkills = new List<SkillModel>();
        }
        public OfferViewModel(OfferModel offer, string idRecruiter)
        {
            Offer = offer;
            IdRecruiter = idRecruiter;
            TopSkills = CalculateTopSkills(offer.Skills);
        }

        private List<SkillModel> CalculateTopSkills(List<SkillModel> skills)
        {
            var topSkills = new List<SkillModel>();
            skills.Sort();
            for(int i = 0 ; i < TopSkillNumber && i <skills.Count ; i++)
            {
                var skill = new SkillModel(skills[i].Name, skills[i].Level);
                topSkills.Add(skill);
            }
            return topSkills;
        }
    }
}