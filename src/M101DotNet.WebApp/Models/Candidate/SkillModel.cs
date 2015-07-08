namespace WebApp.Models.Candidate
{
    public class SkillModel
    {
        public SkillModel(string name, int level)
        {
            Name = name;
            Level = level;
        }

        public SkillModel() { }
 
        public string Name { get; set; }
        public int Level { get; set; }
    }
}