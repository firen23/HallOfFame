using HallOfFame.Dtos;

namespace HallOfFame.Services;

public interface IStorageService
{
    public Task<List<PersonDto>> GetPersonsDtos();

    public Task<PersonDto?> GetPersonDto(long id);

    public Task AddPersonDto(PersonDto personDto);

    public Task<bool> UpdatePersonDto(PersonDto personDto, long id);

    public Task<bool> DeletePersonDto(long id);
}