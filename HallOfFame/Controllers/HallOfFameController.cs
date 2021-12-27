using Microsoft.AspNetCore.Mvc;
using HallOfFame.Data;
using HallOfFame.Services;
using HallOfFame.Dtos;

namespace HallOfFame.Controllers;

[ApiController]
[Route("api/v1")]
public class HallOfFameController : ControllerBase
{
    private readonly ILogger<HallOfFameController> _logger;
    private readonly IStorageService _service;
    
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
            var personsDtos = await _service.GetPersonsDtos();
            return Ok(personsDtos);
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
            var personDto = await _service.GetPersonDto(id);
            if (personDto is null)
            {
                _logger.LogError("{} {}: Person id: {} not found", 
                    Request.Method,
                    Request.Path.Value,
                    id);
                return NotFound();
            }

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
            await _service.AddPersonDto(personDto);
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
            var isSuccess = await _service.UpdatePersonDto(personDto, id);

            if (!isSuccess)
            {
                _logger.LogError("{} {}: Person id: {} not found",
                    Request.Method,
                    Request.Path.Value,
                    id);
                return NotFound();
            }
        
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
            var isSuccess = await _service.DeletePersonDto(id);
        
            if (!isSuccess)
            {
                _logger.LogError("{} {}: Person id: {} not found",
                    Request.Method,
                    Request.Path.Value,
                    id);
                return NotFound();
            }
            
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
