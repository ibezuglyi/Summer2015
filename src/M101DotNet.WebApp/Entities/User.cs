using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApp.Entities
{
    public abstract class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}