

using domain.src.entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


    // obj.Members here prop call
    // public DbSet<Member> Members { get; set; }
    // here obj.Members() like a method call
     public DbSet<Member> Members => Set<Member>();

}