using Microsoft.EntityFrameworkCore;
using HallOfFame.Data;
using HallOfFame.Models;
using HallOfFame.Dtos;
using HallOfFame.Utils;

namespace HallOfFame.Services;

public class StorageService : IStorageService
{
    private readonly HallOfFameContext _context;

    public StorageService(HallOfFameContext context)
    {
        _context = context;
    }

    public async Task<List<PersonDto>> GetPersonsDtos()
    {
        var persons = await GetPersonsWithSkills();

        foreach (var person in persons)
        {
            person.Skills = ModelFilter.GetLastUpdatedSkillRecords(person.Skills);
        }
            
        var personsDto = persons.Select(person => new PersonDto(person)).ToList();
        return personsDto;
    }

    public async Task<PersonDto?> GetPersonDto(long id)
    {
        var person = await GetPersonWithSkills(id);
        if (person is null)
        {
            return null;
        }
        
        person.Skills = ModelFilter.GetLastUpdatedSkillRecords(person.Skills);

        var personDto = new PersonDto(person);
        return personDto;
    }

    public async Task AddPersonDto(PersonDto personDto)
    {
        var person = new Person(personDto);
        await AddPerson(person);
    }

    public async Task<bool> UpdatePersonDto(PersonDto personDto, long id)
    {
        var person = await GetPersonWithSkills(id);

        if (person is null)
        {
            return false;
        }
        
        var oldLastUpdatedSkills = ModelFilter.GetLastUpdatedSkillRecords(person.Skills);
        
        person.Name = personDto.Name;
        person.DisplayName = personDto.DisplayName;
        
        foreach (var newSkill in personDto.Skills)
        {
            var skill = oldLastUpdatedSkills
                .FirstOrDefault(s => s.Name == newSkill.Name && s.Level == newSkill.Level);
            if (skill is null)
            {
                person.Skills.Add(new Skill(newSkill));
            }
        }

        foreach (var oldSkill in oldLastUpdatedSkills)
        {
            var skill = personDto.Skills
                .FirstOrDefault(s => s.Name == oldSkill.Name);
            if (skill is not null) continue;
            var deletedSkill = new Skill()
            {
                Name = oldSkill.Name,
                Level = 0,
                LastUpdated = DateTime.UtcNow
            };
            person.Skills.Add(deletedSkill);
        }
        
        await UpdatePerson(person);
        return true;
    }

    public async Task<bool> DeletePersonDto(long id)
    {
        var person = await GetPerson(id);
        
        if (person is null)
        {
            return false;
        }
    
        await DeletePerson(person);
        return true;
    }
    
    private async Task<List<Person>> GetPersonsWithSkills()
    {
        return await _context.Persons
            .Include(p => p.Skills)
            .ToListAsync();
    }

    private async Task<Person?> GetPerson(long id)
    {
        return await _context.Persons
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    private async Task<Person?> GetPersonWithSkills(long id)
    {
        return await _context.Persons
            .Include(p => p.Skills)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    private async Task AddPerson(Person person)
    {
        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();
    }

    private async Task UpdatePerson(Person person)
    {
        _context.Persons.Update(person);
        await _context.SaveChangesAsync();
    }

    private async Task DeletePerson(Person person)
    {
        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
    }
}
