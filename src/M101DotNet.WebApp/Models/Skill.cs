using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Skill
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Level { get; set; }
    }
}