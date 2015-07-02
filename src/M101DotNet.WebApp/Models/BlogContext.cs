using System;
using System.Collections.Generic;
using System.Configuration;
using MongoDB.Driver;

namespace WebApp.Models
{
    public class BlogContext
    {
        public const string CONNECTION_STRING_NAME = "Blog";
        public const string DATABASE_NAME = "blog";
        public const string USERS_COLLECTION_NAME = "users";
        public const string RECRUITER_USERS_COLLECTION_NAME = "recruiterUsers";
        public const string CANDIDATE_USERS_COLLECTION_NAME = "candidateUsers";

        private static readonly IMongoClient _client;
        private static readonly IMongoDatabase _database;

        static BlogContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[CONNECTION_STRING_NAME].ConnectionString;
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(DATABASE_NAME);
        }

        public IMongoClient Client
        {
            get { return _client; }
        }

        public IMongoCollection<User> Users
        {
            get { return _database.GetCollection<User>(USERS_COLLECTION_NAME); }
        }


        public IMongoCollection<RecruiterUser> RecruiterUsers
        {
            get { return _database.GetCollection<RecruiterUser>(RECRUITER_USERS_COLLECTION_NAME); }
        }

        public IMongoCollection<CandidateUser> CandidateUsers
        {
            get { return _database.GetCollection<CandidateUser>(CANDIDATE_USERS_COLLECTION_NAME); }
        }
    }
}