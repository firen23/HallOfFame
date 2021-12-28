using System.ComponentModel.DataAnnotations;
using HallOfFame.Models;

namespace HallOfFame.Dtos;

public class PersonDto
{
    /// <summary>
    /// The ID of the Person
    /// </summary>
    public long? Id { get; set; }
    
    /// <summary>
    /// The name of the Person
    /// </summary>
    /// <example>John</example>
    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Invalid name length")]
    public string Name { get; set; }
    
    /// <summary>
    /// The displayed name of the Person
    /// </summary>
    /// <example>John Doe</example>
    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Invalid name length")]
    public string DisplayName { get; set; }
    
    /// <summary>
    /// The list of Person's skills
    /// </summary>
    /// <example>[{ "name": "English", "level": 5 }, { "name": "C#", "level": 4 }]</example>
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