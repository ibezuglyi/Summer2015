using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Skill
    {
        [Required (ErrorMessage="Field name is required")]
        public string Name { get; set; }

        [Required (ErrorMessage="Field level is required")]
        [Range(1, 10, ErrorMessage = "Can only be between 1 .. 10")]
        public int Level { get; set; }
    }
}