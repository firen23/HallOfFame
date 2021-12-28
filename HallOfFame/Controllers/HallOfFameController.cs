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

    /// <summary>
    /// Gets the list of all <c>PersonDto</c> objects from the data source
    /// </summary>
    /// <returns>A status code 200 with List of <c>PersonDto</c> objects
    /// or status code 500 if there was an internal error</returns>
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

    /// <summary>
    /// Gets the <c>PersonDto</c> object from the data source by unique ID.
    /// </summary>
    /// <param name="id">Person's ID</param>
    /// <returns>A status code 200 with List of <c>PersonDto</c> objects,
    /// status code 404 if person with that ID was not found
    /// or status code 500 if there was an internal error.</returns>
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

    /// <summary>
    /// Adds the <c>PersonDto</c> object to the data source.
    /// </summary>
    /// <param name="personDto">PersonDto object</param>
    /// <returns>A status code 200
    /// or status code 500 if there was an internal error.</returns>
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

    /// <summary>
    /// Updates the <c>PersonDto</c> object in the data source.
    /// </summary>
    /// <param name="personDto">PersonDto object</param>
    /// <param name="id">Person's ID</param>
    /// <returns>A status code 200,
    /// status code 404 if person with that ID was not found
    /// or status code 500 if there was an internal error.</returns>
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
    
    /// <summary>
    /// Deletes the <c>PersonDto</c> object from the data source.
    /// </summary>
    /// <returns>A status code 200,
    /// status code 404 if person with that ID was not found
    /// or status code 500 if there was an internal error.</returns>
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
