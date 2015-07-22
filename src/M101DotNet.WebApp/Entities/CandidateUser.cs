using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.Entities
{
    public class CandidateUser : User
    {
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime ModificationDate { get; set; }
        
        public string ExperienceDescription { get; set; }

        public int? ExperienceInYears { get; set; }

        public int? Salary { get; set; }

        public List<Skill> Skills { get; set; }

        public CandidateUser()
        {
            Skills = new List<Skill>();
        }

        public bool HasSkills()
        {
            return (Skills.Count > 0);
        }
    }
}