using HallOfFame.Dtos;

namespace HallOfFame.Models;

public class Person
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public ICollection<Skill> Skills { get; set; }

    public Person() {}
    public Person(PersonDto personDto)
    {
        Name = personDto.Name;
        DisplayName = personDto.DisplayName;
        Skills = personDto.Skills.Select(s => new Skill(s)).ToList();
    }
}
