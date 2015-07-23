using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Entities;
using WebApp.Services;
using FluentAssertions;

namespace WebApp.Tests
{
    [TestFixture]
    class ApplicationServiceTests
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
        public async Task CanGetOffersByIdRecruiter()
        {
            var jobOffers = new List<JobOffer>()
            {
                { new JobOffer() { Id="1", Name="First", Salary=100, RecruiterId="17", ModificationDate = DateTime.Now, Skills = new List<Skill>(){ new Skill("C", 9)}}},
                { new JobOffer() { Id="2", Name="Second", Salary=200, RecruiterId="17", ModificationDate = DateTime.Now, Skills = new List<Skill>(){ new Skill("C#", 2)}}},
                { new JobOffer() { Id="3", Name="Third", Salary=300, RecruiterId="17", ModificationDate = DateTime.Now, Skills = new List<Skill>(){ new Skill("C++", 6)}}},
                { new JobOffer() { Id="4", Name="Forth", Salary=400, RecruiterId="17", ModificationDate = DateTime.Now, Skills = new List<Skill>(){ new Skill("CSS", 3)}}},
            };

            dbService
                .Setup(r => r.GetOffersByIdRecruiterSortedByDateAsync(It.IsAny<string>()))
                .Returns(Task<List<JobOffer>>.FromResult(jobOffers));

            var result = await applicationService.GetOffersByIdRecruiterAsync("17");

            jobOffers.ShouldBeEquivalentTo(
                result,
                option => option.WithStrictOrdering()
            );
        }

        [TestCase]
        public async Task CannotGetOffersByIdRecruiter()
        {
            List<JobOffer> jobOffers = null;

            dbService
                .Setup(r => r.GetOffersByIdRecruiterSortedByDateAsync(It.IsAny<string>()))
                .Returns(Task<List<JobOffer>>.FromResult(jobOffers));

            var result = await applicationService.GetOffersByIdRecruiterAsync("17");

            Assert.IsNull(result);
        }

        [TestCase]
        public async Task CanGetOffersByIdRecruiterEmpty()
        {
            var jobOffers = new List<JobOffer>();

            dbService
                .Setup(r => r.GetOffersByIdRecruiterSortedByDateAsync(It.IsAny<string>()))
                .Returns(Task<List<JobOffer>>.FromResult(jobOffers));

            var result = await applicationService.GetOffersByIdRecruiterAsync("17");

            Assert.IsEmpty(result);
        }

        [TestCase]
        public async Task CanGetRecruiterByEmail()
        {
            var recruiterUser = new RecruiterUser() { CompanyDescription = "a", Id = "12345", Name = "b" };
            
            dbService
                .Setup(r => r.GetRecruterByEmailAsync(It.IsAny<string>()))
                .Returns(Task<RecruiterUser>.FromResult(recruiterUser));   

            var result = await applicationService.GetRecruterByEmailAsync("blsabla");

            Assert.AreEqual(result.Id, recruiterUser.Id);
        }

        [TestCase]
        public async Task CannotGetRecruiterByEmail()
        {
            RecruiterUser recruiterUser = null;

            dbService
                .Setup(r => r.GetRecruterByEmailAsync(It.IsAny<string>()))
                .Returns(Task<RecruiterUser>.FromResult(recruiterUser));

            var result = await applicationService.GetRecruterByEmailAsync("blsabla");

            Assert.IsNull(result);
        }

        [TestCase]
        public async Task CanGetJobOfferById()
        {
            JobOffer jobOffer = new JobOffer()
            {
                Name = "AA",
                Salary = 11,
                RecruiterId = "22222",
                Description = "Description",
                Skills = new List<Skill>() { new Skill() { Name = "C++", Level = 5 } }
            };

            dbService
                .Setup(r => r.GetJobOfferByIdAsync(It.IsAny<string>()))
                .Returns(Task<JobOffer>.FromResult(jobOffer));

            var result = await applicationService.GetJobOfferByIdAsync("11111");

            Assert.AreEqual(jobOffer.Name, result.Name);
        }

        [TestCase]
        public async Task CannotGetJobOfferById()
        {
            JobOffer jobOffer = null;

            dbService
                .Setup(r => r.GetJobOfferByIdAsync(It.IsAny<string>()))
                .Returns(Task<JobOffer>.FromResult(jobOffer));

            var applicationService = new ApplicationService(mappingService.Object, dbService.Object);

            var result = await applicationService.GetJobOfferByIdAsync("11111");

            Assert.IsNull(result);
        }

    }
}
