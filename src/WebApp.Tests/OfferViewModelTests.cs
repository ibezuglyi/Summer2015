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
            //arrange
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

            //act
            var result = offerViewModel.CalculateTopSkills(skillModels);

            //assert
            offerViewModel.TopSkills.ShouldBeEquivalentTo(
                result, 
                options => options.WithStrictOrdering()
            );
        }

        [TestCase]
        public void CanCalculateSkillsEmpty()
        {
            //arrange
            List<SkillModel> skillModels = new List<SkillModel>();

            //act
            var result = offerViewModel.CalculateTopSkills(skillModels);

            //assert
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void CannotCalculateSkills()
        {
            //arrange
            List<SkillModel> skillModels = null;

            //act
            var result = offerViewModel.CalculateTopSkills(skillModels);

            //assert
            Assert.IsEmpty(result);
        }
    }
}
