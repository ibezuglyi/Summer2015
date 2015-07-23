using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class SkillSuggestionModel
    {
        public string Query { get; set; }
        public List<string> Suggestions { get; set; }

        public SkillSuggestionModel(string query, List<string> suggestions)
        {
            Query = query;
            Suggestions = suggestions;
        }

        public SkillSuggestionModel()
        {
            Suggestions = new List<string>();
        }
    }
}