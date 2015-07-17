using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WebApp.Models;
using System;

namespace WebApp.Entities
{
    public class JobOffer
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdRecruiter { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime ModificationDate { get; set; }

        [Required]
        public int Salary { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Skill> Skills { get; set; }
        

        public JobOffer()
        {
            Skills = new List<Skill>();
        }

        public JobOffer(string name, int salary, string idRecruitert, List<Skill> skills)
        {
            Name = name;
            Salary = salary;
            IdRecruiter = idRecruitert;
            Skills = skills;
        }
    }
}