using System.ComponentModel.DataAnnotations;

namespace WebApp.Entities
{
    public class Skill
    {
        public string Name { get; set; }

        public string NameToLower { get; set; }

        public int Level { get; set; }

        public Skill() { }

        public Skill(string name, int level)
        {
            Name = name;
            NameToLower = name.ToLower();
            Level = level;
        }
    }
}