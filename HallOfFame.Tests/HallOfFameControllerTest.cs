using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using HallOfFame.Controllers;
using HallOfFame.Dtos;
using HallOfFame.Tests.Services;
using Microsoft.AspNetCore.Mvc;

namespace HallOfFame.Tests;

public class HallOfFameControllerTests
{
    private HallOfFameController _controller;
    
    [SetUp]
    public void Setup()
    {
        var logger = NullLogger<HallOfFameController>.Instance;
        _controller = new HallOfFameController(logger, new TestService());
    }

    [Test]
    public void GetAllPersonsTest()
    {
        var task = _controller.Get();
        task.Wait();
        
        Assert.IsInstanceOf<OkObjectResult>(task.Result.Result);
        var list = task.Result.Result as OkObjectResult;

        Assert.IsInstanceOf<List<PersonDto>>(list?.Value);
        
        var listPersonsDtos = list?.Value as List<PersonDto>;
        Assert.AreEqual(2, listPersonsDtos?.Count);
    }
    
    [Test]
    public void GetPersonByIdTest()
    {
        const int validId = 1;
        const int invalidId = 8;
        
        var okTask = _controller.Get(validId);
        var notFoundTask = _controller.Get(invalidId);
        okTask.Wait();
        notFoundTask.Wait();
        
        Assert.IsInstanceOf<OkObjectResult>(okTask.Result.Result);
        Assert.IsInstanceOf<NotFoundResult>(notFoundTask.Result.Result);
        
        var result = okTask.Result.Result as OkObjectResult;

        Assert.IsInstanceOf<PersonDto>(result?.Value);

        var personDto = result?.Value as PersonDto;
        
        Assert.AreEqual(validId, personDto?.Id);
        Assert.AreEqual("John", personDto?.Name);
        Assert.AreEqual("Doe", personDto?.DisplayName);
        Assert.IsInstanceOf<List<SkillDto>>(personDto?.Skills);
        Assert.AreEqual(2, personDto?.Skills.Count);
    }
    
    [Test]
    public void AddPersonTest()
    {
        var newPersonDto = new PersonDto()
        {
            Id = null,
            Name = "James",
            DisplayName = "Richard",
            Skills = new List<SkillDto>()
            {
                new()
                {
                    Name = "MSSQL",
                    Level = 7
                },
                new()
                {
                    Name = "Postgresql",
                    Level = 5
                }
            }
        };

        var expectedId = 3;
        
        var task = _controller.Post(newPersonDto);
        task.Wait();
        
        Assert.IsInstanceOf<OkResult>(task.Result);

        var checkTask = _controller.Get(expectedId);
        checkTask.Wait();
        Assert.IsInstanceOf<OkObjectResult>(checkTask.Result.Result);

        var result = checkTask.Result.Result as OkObjectResult;

        Assert.IsInstanceOf<PersonDto>(result?.Value);

        var personDto = result?.Value as PersonDto;
        newPersonDto.Id = expectedId;

        AssertArePersonsDtosEqual(newPersonDto, personDto);
    }
    
    [Test]
    public void UpdatePersonByIdTest()
    {
        const int validId = 2;
        const int invalidId = 8;

        var task = _controller.Get(validId);
        task.Wait();
        Assert.IsInstanceOf<OkObjectResult>(task.Result.Result);
        
        var result = task.Result.Result as OkObjectResult;
        Assert.IsInstanceOf<PersonDto>(result?.Value);

        var personDto = result?.Value as PersonDto;
        Assert.NotNull(personDto);
        
        personDto.DisplayName = "King";
        personDto.Skills = new List<SkillDto>()
        {
            new()
            {
                Name = "SQL",
                Level = 5
            },
            new()
            {
                Name = "Python",
                Level = 4
            }
        };
        
        var okTask = _controller.Put(personDto, validId);
        var notFoundTask = _controller.Put(personDto, invalidId);
        okTask.Wait();
        notFoundTask.Wait();
        
        Assert.IsInstanceOf<OkResult>(okTask.Result);
        Assert.IsInstanceOf<NotFoundResult>(notFoundTask.Result);

        var checkTask = _controller.Get(validId);
        checkTask.Wait();
        Assert.IsInstanceOf<OkObjectResult>(checkTask.Result.Result);
        
        var newResult = task.Result.Result as OkObjectResult;
        Assert.IsInstanceOf<PersonDto>(newResult?.Value);

        var updatedPersonDto = newResult?.Value as PersonDto;
        AssertArePersonsDtosEqual(personDto, updatedPersonDto);
    }
    
    [Test]
    public void DeletePersonByIdTest()
    {
        const int validId = 2;
        const int invalidId = 8;
        
        var okTask = _controller.Delete(validId);
        var notFoundTask = _controller.Delete(invalidId);
        okTask.Wait();
        notFoundTask.Wait();
        
        Assert.IsInstanceOf<OkResult>(okTask.Result);
        Assert.IsInstanceOf<NotFoundResult>(notFoundTask.Result);

        var checkTask = _controller.Get(validId);
        checkTask.Wait();
        Assert.IsInstanceOf<NotFoundResult>(checkTask.Result.Result);
    }

    private static void AssertArePersonsDtosEqual(PersonDto? personDto1, PersonDto? personDto2)
    {
        Assert.NotNull(personDto1);
        Assert.NotNull(personDto2);
        Assert.AreEqual(personDto1.Id, personDto2?.Id);
        Assert.AreEqual(personDto1.Name, personDto2?.Name);
        Assert.AreEqual(personDto1.DisplayName, personDto2?.DisplayName);

        foreach (var newSkill in personDto1.Skills)
        {
            var foundSkill = personDto2.Skills
                .FirstOrDefault(s => s.Name == newSkill.Name && s.Level == newSkill.Level);
            Assert.NotNull(foundSkill);
        }
    }
}
