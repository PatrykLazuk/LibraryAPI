using Library.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Api.Tests.Helpers;

// public sealed class TestWebFactory : WebApplicationFactory<Program>
// {
//     private const string DbName = "LibraryApiTests";

//     protected override void ConfigureWebHost(IWebHostBuilder builder)
//     {
//         builder.ConfigureServices(services =>
//         {
//             var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
//             services.Remove(descriptor);

//             services.AddDbContext<AppDbContext>(o =>
//                 o.UseInMemoryDatabase(DbName));
//         });
//     }
// }

public sealed class TestWebFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName;

    public TestWebFactory(string? dbName = null)
        => _dbName = dbName ?? Guid.NewGuid().ToString();   // ← unikalna nazwa

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var desc = services.Single(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            services.Remove(desc);

            services.AddDbContext<AppDbContext>(o =>
                o.UseInMemoryDatabase(_dbName));            // każda instancja = osobna baza
        });
    }
}

public static class TestHelpers
{
    public static void ResetDatabase(this WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }
}

internal static class TestData
{
    public static Models.Book ValidBook(string isbn = "9780000000000") => new()
    {
        Title = "TestTitle",
        Author = "TestAuthor",
        Isbn = isbn
    };
}
