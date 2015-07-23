using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models.Candidate;
using WebApp.Models.Offer;
using FluentAssertions;

namespace WebApp.Tests
{
    [TestFixture]
    class OfferViewModelTests
    {
        private OfferViewModel offerViewModel;

        [SetUp]
        public void SetUp()
        {
            offerViewModel = new OfferViewModel();
        }

        [TestCase]
        public void CanCalculateSkills()
        {
            offerViewModel = new OfferViewModel()
            {
                TopSkills = new List<SkillModel>()
                {
                    new SkillModel("CSS",7),
                    new SkillModel("PHP",6),
                    new SkillModel("HTML",5),
                    new SkillModel("Java",4),
                    new SkillModel("C#",3)
                }
            };

            var skillModels = new List<SkillModel>() 
            { 
                new SkillModel("C++",1),
                new SkillModel("C",2),
                new SkillModel("C#",3),
                new SkillModel("Java",4),
                new SkillModel("HTML",5),
                new SkillModel("PHP",6),
                new SkillModel("CSS",7)
            };

            var result = offerViewModel.CalculateTopSkills(skillModels);

            offerViewModel.TopSkills.ShouldBeEquivalentTo(
                result, 
                options => options.WithStrictOrdering()
            );
        }

        [TestCase]
        public void CanCalculateSkillsEmpty()
        {
            List<SkillModel> skillModels = new List<SkillModel>();

            var result = offerViewModel.CalculateTopSkills(skillModels);

            Assert.IsEmpty(result);
        }

        [TestCase]
        public void CannotCalculateSkills()
        {
            List<SkillModel> skillModels = null;

            var result = offerViewModel.CalculateTopSkills(skillModels);

            Assert.IsEmpty(result);
        }
    }
}
