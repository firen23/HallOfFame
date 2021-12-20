using Microsoft.AspNetCore.Mvc;
using HallOfFame.Data;
using HallOfFame.Services;
using HallOfFame.Models;
using HallOfFame.Dtos;

namespace HallOfFame.Controllers;

[ApiController]
[Route("api/v1")]
public class HallOfFameController : ControllerBase
{
    private readonly ILogger<HallOfFameController> _logger;
    private readonly StorageService _service;
    
    public HallOfFameController(ILogger<HallOfFameController> logger, HallOfFameContext context)
    {
        _logger = logger;
        _service = new StorageService(context);
    }

    [Route("persons")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonDto>>> Get()
    {
        try
        {
            var persons = await _service.GetPersonsWithSkills();

            foreach (var person in persons)
            {
                person.Skills = _service.GetOldSkillRecords(person.Skills);
            }
            
            var personsDto = persons.Select(person => new PersonDto(person)).ToList();
            return Ok(personsDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{} {}: Unable to load persons",
                Request.Method,
                Request.Path.Value);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Route("person/{id:long}")]
    [HttpGet]
    public async Task<ActionResult<PersonDto>> Get(long id)
    {
        try
        {
            var person = await _service.GetPersonWithSkills(id);
            if (person is null)
            {
                _logger.LogError("{} {}: Person id: {} not found", 
                    Request.Method,
                    Request.Path.Value,
                    id);
                return NotFound();
            }
            
            person.Skills = _service.GetOldSkillRecords(person.Skills);
            
            var personDto = new PersonDto(person);
        
            return Ok(personDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{} {}: Unable to load person",
                Request.Method,
                Request.Path.Value);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Route("person")]
    [HttpPost]
    public async Task<ActionResult> Post(PersonDto personDto)
    {
        try
        {
            var person = new Person(personDto);
            await _service.AddPerson(person);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{} {}: Failed to add Person", 
                Request.Method,
                Request.Path.Value);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Route("person/{id:long}")]
    [HttpPut]
    public async Task<ActionResult> Put(PersonDto personDto, long id)
    {
        try
        {
            var person = await _service.GetPersonWithSkills(id);

            if (person is null)
            {
                _logger.LogError("{} {}: Person id: {} not found",
                    Request.Method,
                    Request.Path.Value,
                    id);
                return NotFound();
            }
        
            var oldSkills = _service.GetOldSkillRecords(person.Skills);

            person.Name = personDto.Name;
            person.DisplayName = personDto.DisplayName;

            foreach (var newSkill in personDto.Skills)
            {
                var skill = oldSkills
                    .FirstOrDefault(s => s.Name == newSkill.Name && s.Level == newSkill.Level);
                if (skill is null)
                {
                    person.Skills.Add(new Skill(newSkill));
                }
            }
        
            await _service.UpdatePerson(person);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{} {}: Failed to update Person id: {}", 
                Request.Method,
                Request.Path.Value,
                id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [Route("person/{id:long}")]
    [HttpDelete]
    public async Task<ActionResult> Delete(long id)
    {
        try
        {
            var person = await _service.GetPerson(id);
        
            if (person is null)
            {
                _logger.LogError("{} {}: Person id: {} not found",
                    Request.Method,
                    Request.Path.Value,
                    id);
                return NotFound();
            }
        
            await _service.DeletePerson(person);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{} {}: Failed to delete Person id: {}", 
                Request.Method,
                Request.Path.Value,
                id);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
