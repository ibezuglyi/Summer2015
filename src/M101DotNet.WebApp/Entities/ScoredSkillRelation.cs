using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApp.Entities
{
    public class ScoredSkillRelation
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReferenceId { get; set; }
        public string Type { get; set; }
        public string ReferenceSkillCode { get; set; }
        public string SkillCode { get; set; }
        public double Score { get; set; }
    }
}