using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HallOfFame.Dtos;
using HallOfFame.Models;
using HallOfFame.Services;


namespace HallOfFame.Tests.Services;

public class TestService : IStorageService
{
    private List<Person> _persons;
    private long _index = 1;
    
    public TestService()
    {
        _persons = new List<Person>
        {
            new()
            {
                Id = _index++,
                Name = "John",
                DisplayName = "Doe",
                Skills = new List<Skill>()
                {
                    new()
                    {
                        Id = 1,
                        PersonId = 1,
                        Name = "English",
                        Level = 5,
                        LastUpdated = DateTime.UtcNow
                    },
                    new()
                    {
                        Id = 2,
                        PersonId = 1,
                        Name = "C#",
                        Level = 4,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            },
            new()
            {
                Id = _index++,
                Name = "Alex",
                DisplayName = "Smith",
                Skills = new List<Skill>()
                {
                    new()
                    {
                        Id = 3,
                        PersonId = 2,
                        Name = "English",
                        Level = 7,
                        LastUpdated = DateTime.UtcNow
                    },
                    new()
                    {
                        Id = 4,
                        PersonId = 2,
                        Name = "C++",
                        Level = 6,
                        LastUpdated = DateTime.UtcNow
                    }
                }
            }
        };
    }

    public async Task<List<PersonDto>> GetPersonsDtos()
    {
        var personsDtos = _persons.Select(person => new PersonDto(person)).ToList();
        return personsDtos;
    }

    public async Task<PersonDto?> GetPersonDto(long id)
    {
        var person = _persons.FirstOrDefault(p => p.Id == id);
        if (person is null)
        {
            return null;
        }

        return new PersonDto(person);
    }

    public async Task AddPersonDto(PersonDto personDto)
    {
        var person = new Person(personDto)
        {
            Id = _index++
        };
        
        _persons.Add(person);
    }

    public async Task<bool> UpdatePersonDto(PersonDto personDto, long id)
    {
        var person = _persons.FirstOrDefault(p => p.Id == id);

        if (person is null)
        {
            return false;
        }
        
        person.Name = personDto.Name;
        person.DisplayName = personDto.DisplayName;

        person.Skills = personDto.Skills.Select(skillDto => new Skill(skillDto)).ToList();

        return true;
    }

    public async Task<bool> DeletePersonDto(long id)
    {
        var oldPerson = _persons.FirstOrDefault(p => p.Id == id);

        if (oldPerson is null)
        {
            return false;
        }

        _persons.Remove(oldPerson);
        return true;
    }
}