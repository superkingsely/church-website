

// using ChurchWebsite.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // public DbSet<Member> Members { get; set; }
    //  public DbSet<Member> Members => Set<Member>();

}