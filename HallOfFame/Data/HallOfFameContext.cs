using HallOfFame.Models;
using Microsoft.EntityFrameworkCore;

namespace HallOfFame.Data;

public class HallOfFameContext : DbContext
{
    public HallOfFameContext(DbContextOptions<HallOfFameContext> options) : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Skill> Skills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().ToTable("Person");
        modelBuilder.Entity<Skill>().ToTable("Skill");
    }
}
