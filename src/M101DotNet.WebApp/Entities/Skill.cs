using System.ComponentModel.DataAnnotations;

namespace WebApp.Entities
{
    public class Skill
    {
        public string Name { get; set; }

        public string NameToLower { get; set; }

        public int Level { get; set; }
    }
}