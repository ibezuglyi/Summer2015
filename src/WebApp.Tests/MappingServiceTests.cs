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
            string query = "C";
            List<string> hints = new List<string>();
            var skillSuggestionModelExpected = new SkillSuggestionModel()
            {
                Query = query,
                Suggestions = hints,
            };

            var skillSuggestionModel = mappingService.MapToSkillSugestionModel(query, hints);

            skillSuggestionModel.ShouldBeEquivalentTo(skillSuggestionModelExpected);
        }

        [TestCase]
        public void CanMapToSkillSuggestionModelWhenQueryIsNull()
        {
            string query = null;
            List<string> hints = new List<string>();
            var skillSuggestionModelExpected = new SkillSuggestionModel()
            {
                Query = query,
                Suggestions = hints,
            };

            var skillSuggestionModel = mappingService.MapToSkillSugestionModel(query, hints);

            skillSuggestionModel.ShouldBeEquivalentTo(skillSuggestionModelExpected);
        }

        [TestCase]
        public void CanMapToSkillSuggestionModelWhenHintsAreNull()
        {
            string query = "C";
            List<string> hints = null;
            var skillSuggestionModelExpected = new SkillSuggestionModel()
            {
                Query = query,
                Suggestions = hints,
            };

            var skillSuggestionModel = mappingService.MapToSkillSugestionModel(query, hints);

            skillSuggestionModel.ShouldBeEquivalentTo(skillSuggestionModelExpected);
        }
    }
}
