using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.Services;

namespace WebApp.Tests
{
    class GetMeasureScoreTests
    {
        private Moq.Mock<IMappingService> mappingService;
        private Moq.Mock<IDatabaseService> dbService;
        public ApplicationService applicationService;

        [SetUp]
        public void SetUp()
        {
            mappingService = new Mock<IMappingService>();
            dbService = new Mock<IDatabaseService>();
            applicationService = new ApplicationService(mappingService.Object, dbService.Object);
        }

        [TestCase]
        public void GetMeasureScoreBetweenCandidateAndOfferPerfect()
        {
            var referenceSkills = new List<Skill>() 
            {
                new Skill("C", 2),
                new Skill("C#", 3),
                new Skill("Java", 4),
                new Skill("HTML", 5),
                new Skill("PHP", 6)
            };

            var skills = new List<Skill>()
            {
                new Skill("C", 2),
                new Skill("C#", 3),
                new Skill("Java", 4),
                new Skill("HTML", 5),
                new Skill("PHP", 6)
            };

            double expectedResult = 1.0;

            var result = applicationService.MeasureScoreBetweenCandidateAndOffer(referenceSkills, skills);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase]
        public void GetMeasureScoreBetweenCandidateAndOfferOne()
        {
            var referenceSkills = new List<Skill>() 
            {
                new Skill("C", 2),
                new Skill("C#", 3),
                new Skill("Java", 4),
                new Skill("HTML", 5),
                new Skill("PHP", 6)
            };

            var skills = new List<Skill>()
            {
                new Skill("C", 1),
                new Skill("C#", 2),
                new Skill("Java", 3),
                new Skill("HTML", 4),
                new Skill("PHP", 5)
            };

            double expectedResult = 0.9;

            var result = applicationService.MeasureScoreBetweenCandidateAndOffer(referenceSkills, skills);
            var roundedResult = Math.Round(result, 2);

            Assert.AreEqual(expectedResult, roundedResult);
        }

        [TestCase]
        public void GetMeasureScoreBetweenCandidateAndOfferTwo()
        {
            var referenceSkills = new List<Skill>() 
            {
                new Skill("C", 2),
                new Skill("C#", 3),
                new Skill("Java", 4),
                new Skill("HTML", 5),
                new Skill("PHP", 6)
            };

            var skills = new List<Skill>()
            {
                new Skill("C", 3),
                new Skill("C#", 4),
                new Skill("Java", 5),
                new Skill("HTML", 6),
                new Skill("PHP", 7)
            };

            double expectedResult = 0.92;

            var result = applicationService.MeasureScoreBetweenCandidateAndOffer(referenceSkills, skills);
            var roundedResult = Math.Round(result, 2);

            Assert.AreEqual(expectedResult, roundedResult);
        }

        [TestCase]
        public void GetMeasureScoreBetweenCandidateAndOfferThree()
        {
            var referenceSkills = new List<Skill>() 
            {
                new Skill("C", 2),
                new Skill("C#", 3),
                new Skill("Java", 4),
                new Skill("HTML", 5)
            };

            var skills = new List<Skill>()
            {
                new Skill("C", 3),
                new Skill("C#", 1)
            };

            double expectedResult = 0.43;

            var result = applicationService.MeasureScoreBetweenCandidateAndOffer(referenceSkills, skills);
            var roundedResult = Math.Round(result, 2);

            Assert.AreEqual(expectedResult, roundedResult);
        }

        [TestCase]
        public void GetMeasureScoreBetweenCandidateAndOfferFour()
        {
            var referenceSkills = new List<Skill>() 
            {
                new Skill("C", 2),
                new Skill("C#", 3)
            };

            var skills = new List<Skill>()
            {
                new Skill("C", 3),
                new Skill("C#", 1),
                new Skill("Java", 5),
                new Skill("HTML", 6)
            };

            double expectedResult = 0.86;

            var result = applicationService.MeasureScoreBetweenCandidateAndOffer(referenceSkills, skills);
            var roundedResult = Math.Round(result, 2);

            Assert.AreEqual(expectedResult, roundedResult);
        }

        [TestCase]
        public void GetMeasureScoreBetweenCandidateAndOfferZero()
        {
            var referenceSkills = new List<Skill>() 
            {
                new Skill("C", 2),
                new Skill("C#", 3)
            };

            var skills = new List<Skill>()
            {
                new Skill("Java", 5),
                new Skill("HTML", 6)
            };

            double expectedResult = 0;

            var result = applicationService.MeasureScoreBetweenCandidateAndOffer(referenceSkills, skills);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase]
        public void GetMeasureScoreBetweenCandidateAndOfferZeroTwo()
        {
            var referenceSkills = new List<Skill>() 
            {
                new Skill("C", 2),
                new Skill("C#", 3)
            };

            var skills = new List<Skill>();

            double expectedResult = 0;

            var result = applicationService.MeasureScoreBetweenCandidateAndOffer(referenceSkills, skills);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase]
        public void GetMeasureScoreBetweenCandidateAndOfferEmpty()
        {
            var referenceSkills = new List<Skill>();

            var skills = new List<Skill>();

            double expectedResult = 0;

            var result = applicationService.MeasureScoreBetweenCandidateAndOffer(referenceSkills, skills);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase]
        public void GetMeasureScoreBetweenCandidateAndOfferNulls()
        {
            List<Skill> referenceSkills = null;

            List<Skill> skills = null;

            double expectedResult = 0;

            var result = applicationService.MeasureScoreBetweenCandidateAndOffer(referenceSkills, skills);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
