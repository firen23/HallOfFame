using System.ComponentModel.DataAnnotations;
using HallOfFame.Models;

namespace HallOfFame.Dtos;

public class PersonDto
{
    public long? Id { get; set; }
    
    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Invalid name length")]
    public string Name { get; set; }
    
    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Invalid name length")]
    public string DisplayName { get; set; }
    public List<SkillDto> Skills { get; set; }

    public PersonDto() {}
    public PersonDto(Person person)
    {
        Id = person.Id;
        Name = person.Name;
        DisplayName = person.DisplayName;
        Skills = person.Skills.Select(s => new SkillDto(s)).ToList();
    }
}