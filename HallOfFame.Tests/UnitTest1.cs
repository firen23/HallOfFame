using System;
using System.Collections.Generic;
using System.Linq;
using HallOfFame.Dtos;
using NUnit.Framework;
using HallOfFame.Models;

namespace HallOfFame.Tests;

public class ModelTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestModels()
    {
        var person = GetTestPerson();
        var personDto = new PersonDto(person);
        Assert.True(PersonsDtoAreEqual(personDto, GetTestPersonDto()));
    }

    private bool PersonsAreEqual(Person person1, Person person2)
    {
        var result = true;
        result &= person1.Id == person2.Id;
        result &= person1.Name == person2.Name;
        result &= person1.DisplayName == person2.DisplayName;
        result &= person1.Skills.Count == person2.Skills.Count;

        if (!result) return result;
        foreach (var skill1 in person1.Skills)
        {
            var found = person2.Skills
                .Select(skill2 => SkillsAreEqual(skill2, skill1))
                .Single();
            if (found) continue;
            result = false;
            break;
        }

        return result;
    }

    private static bool SkillsAreEqual(Skill skill1, Skill skill2)
    {
        var result = true;
        result &= skill1.Id == skill2.Id;
        result &= skill1.PersonId == skill2.PersonId;
        result &= skill1.Name == skill2.Name;
        result &= skill1.Level == skill2.Level;
        result &= skill1.LastUpdated == skill2.LastUpdated;
        return result;
    }
    
    private static bool PersonsDtoAreEqual(PersonDto person1, PersonDto person2)
    {
        var result = true;
        result &= person1.Id == person2.Id;
        result &= person1.Name == person2.Name;
        result &= person1.DisplayName == person2.DisplayName;
        result &= person1.Skills.Count == person2.Skills.Count;

        if (!result) return result;
        foreach (var skill1 in person1.Skills)
        {
            var found = person2.Skills
                .Find(skill2 => SkillsDtoAreEqual(skill2, skill1));
            if (found is not null) continue;
            result = false;
            break;
        }

        return result;
    }

    private static bool SkillsDtoAreEqual(SkillDto skill1, SkillDto skill2)
    {
        var result = true;
        result &= skill1.Name == skill2.Name;
        result &= skill1.Level == skill2.Level;
        return result;
    }

    private static Person GetTestPerson()
    {
        return new Person()
        {
            Id = 1,
            Name = "John",
            DisplayName = "Doe",
            Skills = new Skill[]
            {
                new Skill()
                {
                    PersonId = 1,
                    Name = "English",
                    Level = 5,
                    LastUpdated = new DateTime(2021, 12, 01, 12, 1, 05)
                },
                new Skill()
                {
                    PersonId = 1,
                    Name = "C#",
                    Level = 4,
                    LastUpdated = new DateTime(2021, 12, 12, 14, 58, 01)
                }
            }
        };
    }

    private static PersonDto GetTestPersonDto()
    {
        return new PersonDto()
        {
            Id = 1,
            Name = "John",
            DisplayName = "Doe",
            Skills = new List<SkillDto>()
            {
                new()
                {
                    Name = "English",
                    Level = 5
                },
                new()
                {
                    Name = "C#",
                    Level = 4
                }
            }
        };
    }
}
