using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApp.Models
{
    public class Skill
    {
        public string Name { get; set; }
        public byte Level { get; set; }
    }
}