using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class SkillHintsModel
    {
        public string query { get; set; }
        public List<string> suggestions { get; set; }

        public SkillHintsModel(string _query, List<string> _suggestions)
        {
            this.query = _query;
            this.suggestions = _suggestions;
        }

    }
}