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
using WebApp.Models;

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
            var skillModelList = new List<SkillModel>()
            {
                new SkillModel("C", 2),
                new SkillModel("C#", 3),
                new SkillModel("Java", 4),
                new SkillModel("HTML", 5),
                new SkillModel("PHP", 6)
            };

            var skillList = new List<Skill>()
            {
                new Skill("C", 2),
                new Skill("C#", 3),
                new Skill("Java", 4),
                new Skill("HTML", 5),
                new Skill("PHP", 6)
            };

            var result = mappingService.MapSkillModelsToSkills(skillModelList);

            skillModelList.ShouldBeEquivalentTo(
                result, 
                option => option.WithStrictOrdering()
            );
        }

        [TestCase]
        public void CanMapSkillModelsToSkillsEmpty()
        {
            var skillModelList = new List<SkillModel>();

            var result = mappingService.MapSkillModelsToSkills(skillModelList);

            Assert.IsEmpty(result);
        }

        [TestCase]
        public void CannotMapSkillModelsToSkills()
        {
            List<SkillModel> skillModelList = null;

            var result = mappingService.MapSkillModelsToSkills(skillModelList);

            Assert.IsEmpty(result);
        }

        [TestCase]
        public void CanMapToSkillSuggestionModel()
        {
            string testQuery = "C";
            List<string> hints = new List<string>();
            var skillSuggestionModelExpected = new SkillSuggestionModel()
            {
                query = testQuery,
                suggestions = hints,
            };

            var skillSuggestionModel = mappingService.MapToSkillSugestionModel(testQuery, hints);

            skillSuggestionModel.ShouldBeEquivalentTo(skillSuggestionModelExpected);
        }

        [TestCase]
        public void CanMapToSkillSuggestionModelWhenQueryIsNull()
        {
            string testQuery = null;
            List<string> hints = new List<string>();
            var skillSuggestionModelExpected = new SkillSuggestionModel()
            {
                query = testQuery,
                suggestions = hints,
            };

            var skillSuggestionModel = mappingService.MapToSkillSugestionModel(testQuery, hints);

            skillSuggestionModel.ShouldBeEquivalentTo(skillSuggestionModelExpected);
        }

        [TestCase]
        public void CanMapToSkillSuggestionModelWhenHintsAreNull()
        {
            string testQuery = "C";
            List<string> hints = null;
            var skillSuggestionModelExpected = new SkillSuggestionModel()
            {
                query = testQuery,
                suggestions = hints,
            };

            var skillSuggestionModel = mappingService.MapToSkillSugestionModel(testQuery, hints);

            skillSuggestionModel.ShouldBeEquivalentTo(skillSuggestionModelExpected);
        }
    }
}
