using System.ComponentModel.DataAnnotations;
using HallOfFame.Models;

namespace HallOfFame.Dtos;

public class SkillDto
{
    /// <summary>
    /// The name of the Skill
    /// </summary>
    /// <example>English</example>
    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Invalid name length")]
    public string Name { get; set; }
    
    /// <summary>
    /// The level of the Skill (1 - 10)
    /// </summary>
    /// <example>8</example>
    [Required]
    [Range(1, 10, ErrorMessage = "Invalid level")]
    public byte Level { get; set; }

    public SkillDto() {}

    public SkillDto(Skill skill)
    {
        Name = skill.Name;
        Level = skill.Level ?? 0;
    }
}
