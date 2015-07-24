using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class SkillSuggestionModel
    {
        public string query { get; set; }
        public List<string> suggestions { get; set; }

        public SkillSuggestionModel(string Query, List<string> Suggestions)
        {
            query = Query;
            suggestions = Suggestions;
        }

        public SkillSuggestionModel()
        {
            suggestions = new List<string>();
        }
    }
}