using System;
using System.ComponentModel.DataAnnotations;
namespace WebApp.Models.Candidate
{
    public class SkillModel :IComparable
    {
        public SkillModel(string name, int level)
        {
            Name = name;
            Level = level;
        }

        public SkillModel() { }

        [Required(ErrorMessage = "Field name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field level is required")]
        [Range(1, 10, ErrorMessage = "Can only be between 1 .. 10")]
        public int Level { get; set; }

        int IComparable.CompareTo(object o)
        {
            SkillModel skill = (SkillModel)o;
            if (this.Level < skill.Level) return 1;
            else if (this.Level > skill.Level) return -1;
            else return 0;
        }
    }
}