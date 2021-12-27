using HallOfFame.Models;

namespace HallOfFame.Utils;

public static class ModelFilter
{
    public static List<Skill> GetLastUpdatedSkillRecords(IEnumerable<Skill> skills)
    {
        return skills
            .GroupBy(s => s.Name)
            .Select(@group => @group
                .OrderByDescending(s => s.LastUpdated)
                .First())
            .Where(s => s.Level > 0)
            .ToList();
    }
}
