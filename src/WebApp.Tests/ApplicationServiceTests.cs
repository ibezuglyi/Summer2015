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
    [TestFixture]
    class ApplicationServiceTests
    {
        private Moq.Mock<IMappingService> mappingService;
        private Moq.Mock<IDatabaseService> dbService;
        
        [SetUp]
        public void SetUp()
        {
            mappingService = new Mock<IMappingService>();
            dbService = new Mock<IDatabaseService>();
        }

        [TestCase]
        public async Task CanGetRecruiterByEmail()
        {
            //arrange
            var recruiterUser = new RecruiterUser() { CompanyDescription = "a", Id = "12345", Name = "b" };

            dbService
                .Setup(r => r.GetRecruterByEmailAsync(It.IsAny<string>()))
                .Returns(Task<RecruiterUser>.FromResult(recruiterUser));

            var applicationService = new ApplicationService(mappingService.Object, dbService.Object);

            //act
            var result = await applicationService.GetRecruterByEmailAsync("blsabla");

            //assert
            Assert.AreEqual(result.Id, recruiterUser.Id);
        }

        [TestCase]
        public async Task CannotGetRecruiterByEmail()
        {
            //arrange
            RecruiterUser recruiterUser = null;

            dbService
                .Setup(r => r.GetRecruterByEmailAsync(It.IsAny<string>()))
                .Returns(Task<RecruiterUser>.FromResult(recruiterUser));

            var applicationService = new ApplicationService(mappingService.Object, dbService.Object);
            //act
            var result = await applicationService.GetRecruterByEmailAsync("blsabla");

            //assert
            Assert.IsNull(result);
        }

        [TestCase]
        public async Task CanGetJobOfferById()
        {
            //arrange
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

            var applicationService = new ApplicationService(mappingService.Object, dbService.Object);
            //act

            var result = await applicationService.GetJobOfferByIdAsync("11111");

            //assert
            Assert.AreEqual(jobOffer.Name, result.Name);
        }

        [TestCase]
        public async Task CannotGetJobOfferById()
        {
            //arrange
            JobOffer jobOffer = null;

            dbService
                .Setup(r => r.GetJobOfferByIdAsync(It.IsAny<string>()))
                .Returns(Task<JobOffer>.FromResult(jobOffer));

            var applicationService = new ApplicationService(mappingService.Object, dbService.Object);
            //act

            var result = await applicationService.GetJobOfferByIdAsync("11111");

            //assert
            Assert.IsNull(result);
        }

    }
}
