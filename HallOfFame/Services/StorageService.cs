using HallOfFame.Data;
using HallOfFame.Models;
using Microsoft.EntityFrameworkCore;

namespace HallOfFame.Services;

public class StorageService
{
    private readonly HallOfFameContext _context;

    public StorageService(HallOfFameContext context)
    {
        _context = context;
    }

    public async Task<List<Person>> GetPersonsWithSkills()
    {
        return await _context.Persons
            .Include(p => p.Skills)
            .ToListAsync();
    }

    public async Task<Person?> GetPerson(long id)
    {
        return await _context.Persons
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<Person?> GetPersonWithSkills(long id)
    {
        return await _context.Persons
            .Include(p => p.Skills)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddPerson(Person person)
    {
        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePerson(Person person)
    {
        _context.Persons.Update(person);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePerson(Person person)
    {
        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
    }
    
    public List<Skill> GetOldSkillRecords(IEnumerable<Skill> skills)
    {
        return skills
            .GroupBy(s => s.Name)
            .Select(@group => @group
                .OrderByDescending(s => s.LastUpdated)
                .First())
            .ToList();
    }
}
