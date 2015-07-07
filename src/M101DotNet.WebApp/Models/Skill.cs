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
        [Range(1, 10, ErrorMessage = "Can only be between 1 .. 10")]
        public int Level { get; set; }
    }
}