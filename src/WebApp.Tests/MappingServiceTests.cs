using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.Models.Candidate;
using WebApp.Services;
using FluentAssertions;

namespace WebApp.Tests
{
    [TestFixture]
    class MappingServiceTests
    {
        MappingService mappingService;

        [SetUp]
        public void SetUp()
        {
            mappingService = new MappingService();
        }

        [TestCase]
        public void CanMapSkillModelsToSkills()
        {
            //arrange
            var skillModelList = new List<SkillModel>()
            {
                { new SkillModel() { Name = "C", Level = 2 } },
                { new SkillModel() { Name = "C#", Level = 3 } },
                { new SkillModel() { Name = "Java", Level = 4 } },
                { new SkillModel() { Name = "HTML", Level = 5 } },
                { new SkillModel() { Name = "PHP", Level = 6 } }
            };

            var skillList = new List<Skill>()
            {
                { new Skill() { Name = "C", Level = 2 } },
                { new Skill() { Name = "C#", Level = 3 } },
                { new Skill() { Name = "Java", Level = 4 } },
                { new Skill() { Name = "HTML", Level = 5 } },
                { new Skill() { Name = "PHP", Level = 6 } }
            };

            //act
            var result = mappingService.MapSkillModelsToSkills(skillModelList);

            //assert
            skillModelList.ShouldBeEquivalentTo(
                result, 
                option => option.WithStrictOrdering()
            );
        }

        [TestCase]
        public void CanMapSkillModelsToSkillsEmpty()
        {
            //arrange
            var skillModelList = new List<SkillModel>();

            //act
            var result = mappingService.MapSkillModelsToSkills(skillModelList);

            //assert
            Assert.IsEmpty(result);
        }

        [TestCase]
        public void CannotMapSkillModelsToSkills()
        {
            //arrange
            List<SkillModel> skillModelList = null;

            //act
            var result = mappingService.MapSkillModelsToSkills(skillModelList);

            //assert
            Assert.IsEmpty(result);
        }
    }
}
