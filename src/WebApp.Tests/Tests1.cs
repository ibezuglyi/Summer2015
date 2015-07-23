using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WebApp.Entities;
using WebApp.Services;

namespace WebApp.Tests
{
    [TestFixture]
    public class Test1
    {
        [TestCase]
        public void CanRunTest()
        {
            //AAA

            //Arrange
            var x = 2;
            var y = 2;
            var expectedSum = 4;

            //act
            var sum = x + y;

            //assert 
            Assert.AreEqual(sum, expectedSum);

        }

        [TestCase]
        public async Task CanGetRecruiterModelByEmail()
        {
            //arrange
            var mappingService = new Mock<IMappingService>();
            var dbService = new Mock<IDatabaseService>();


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
        public async Task CannotGetRecruiterModelByEmail()
        {
            //arrange
            var mappingService = new Mock<IMappingService>();
            var dbService = new Mock<IDatabaseService>();

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
    }
}
