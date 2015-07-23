using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Services;
using WebApp.Helpers;
using MongoDB.Driver;
using WebApp.Entities;
using System.Linq.Expressions;
using System.Threading;

namespace WebApp.Tests
{
    [TestFixture]
    class JobifyMongoCollectionExtensionsTests
    {
        private Mock<IMongoCollection<CandidateUser>> mongoCollection;

        [SetUp]
        public void SetUp()
        {
            mongoCollection = new Mock<IMongoCollection<CandidateUser>>();
        }

        [TestCase]
        public async Task CannotFindByIdWhenIdIsNull()
        {
            string id = null;
            var candidate = await Helpers.JobifyMongoCollectionExtensions.FindById<CandidateUser>(mongoCollection.Object, r => r.Id == id);
            Assert.IsNull(candidate);
        }

        [TestCase]
        public async Task CannotFindByIdWhenIdIsNotValid()
        {
            string id = "alamakota";
            var candidate = await Helpers.JobifyMongoCollectionExtensions.FindById<CandidateUser>(mongoCollection.Object, r => r.Id == id);
            Assert.IsNull(candidate);
        }

    }
}
