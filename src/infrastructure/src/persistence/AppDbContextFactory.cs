using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        var projectRoot = Directory.GetParent(
            Directory.GetCurrentDirectory())?.Parent?.FullName;

             var databasePath = Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory,
                "../../../../../src/api/ChurchWebsite.db"));

        // var databasePath = Path.Combine(
        //     projectRoot!,
        //     "api",
        //     "ChurchWebsite.db");

        optionsBuilder.UseSqlite($"Data Source={databasePath}");

        return new AppDbContext(optionsBuilder.Options);
    }
}

// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;

// namespace Infrastructure.Persistence;

// public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
// {
//     public AppDbContext CreateDbContext(string[] args)
//     {
//         var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

//         var databasePath = Path.Combine(
//             Directory.GetCurrentDirectory(),
//             "..",
//             "api",
//             "ChurchWebsite.db");

//         optionsBuilder.UseSqlite($"Data Source={databasePath}");

//         return new AppDbContext(optionsBuilder.Options);
//     }
// }

// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;

// namespace Infrastructure.Persistence;

// public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
// {
//     public AppDbContext CreateDbContext(string[] args)
//     {
//         var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

//         optionsBuilder.UseSqlite(
//             "Data Source=churchwebsite.db");

//         return new AppDbContext(optionsBuilder.Options);
//     }
// }