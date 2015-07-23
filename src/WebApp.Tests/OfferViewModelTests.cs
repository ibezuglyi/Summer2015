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
                    { new SkillModel() { Name = "CSS", Level = 7 } },
                    { new SkillModel() { Name = "PHP", Level = 6 } },
                    { new SkillModel() { Name = "HTML", Level = 5 } },
                    { new SkillModel() { Name = "Java", Level = 4 } },
                    { new SkillModel() { Name = "C#", Level = 3 } }
                }
            };

            var skillModels = new List<SkillModel>() 
            { 
                { new SkillModel() { Name = "C++", Level = 1 } },
                { new SkillModel() { Name = "C", Level = 2 } },
                { new SkillModel() { Name = "C#", Level = 3 } },
                { new SkillModel() { Name = "Java", Level = 4 } },
                { new SkillModel() { Name = "HTML", Level = 5 } },
                { new SkillModel() { Name = "PHP", Level = 6 } },
                { new SkillModel() { Name = "CSS", Level = 7 } }
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
