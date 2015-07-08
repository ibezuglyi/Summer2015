using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class JobOffer
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdRecruiter { get; set; }
        
        [Required]
        public int Salary { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Skill> Skills { get; set; }
        

        public JobOffer()
        {
            Skills = new List<Skill>();
        }
    }
}