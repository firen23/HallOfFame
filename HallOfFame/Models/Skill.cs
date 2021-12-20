using HallOfFame.Dtos;

namespace HallOfFame.Models;

public class Skill
{
    public long Id { get; set; }
    public long PersonId { get; set; }
    public string Name { get; set; }
    public byte? Level { get; set; }
    public DateTime LastUpdated { get; set; }
    
    public Skill() {}
    
    public Skill(SkillDto skillDto)
    {
        Name = skillDto.Name;
        Level = skillDto.Level;
        LastUpdated = DateTime.UtcNow;
    }
}
