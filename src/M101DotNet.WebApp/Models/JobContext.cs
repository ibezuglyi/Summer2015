using System;
using System.Collections.Generic;
using System.Configuration;
using MongoDB.Driver;
using WebApp.Entities;

namespace WebApp.Models
{
    public class JobContext
    {
        public const string CONNECTION_STRING_NAME = "hotsummer";
        public const string DATABASE_NAME = "jobify";
        public const string RECRUITER_USERS_COLLECTION_NAME = "recruiters";
        public const string CANDIDATE_USERS_COLLECTION_NAME = "candidates";
        public const string OFFERS_COLLECTION_NAME = "offers";

        private static readonly IMongoClient _client;
        private static readonly IMongoDatabase _database;

        static JobContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[CONNECTION_STRING_NAME].ConnectionString;
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(DATABASE_NAME);
        }

        public IMongoClient Client
        {
            get { return _client; }
        }

        public IMongoCollection<RecruiterUser> RecruiterUsers
        {
            get { return _database.GetCollection<RecruiterUser>(RECRUITER_USERS_COLLECTION_NAME); }
        }

        public IMongoCollection<CandidateUser> CandidateUsers
        {
            get { return _database.GetCollection<CandidateUser>(CANDIDATE_USERS_COLLECTION_NAME); }
        }

        public IMongoCollection<JobOffer> JobOffers
        {
            get
            {
                return _database.GetCollection<JobOffer>(OFFERS_COLLECTION_NAME);
                
            }
        }
    }
}