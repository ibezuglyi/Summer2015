using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApp.Models
{
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }


    public class RecruiterUser : User
    {
        public string PhoneNumber { get; set; }

        public string CompanyProfile { get; set; }  
    }

    public class CandidateUser : User
    {
        //keep an open mind :)
    }
}