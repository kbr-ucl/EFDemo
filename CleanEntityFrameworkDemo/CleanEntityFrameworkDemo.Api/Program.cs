using CleanEntityFrameworkDemo.Application;
using CleanEntityFrameworkDemo.Infrastructure.Database;
using CleanEntityFrameworkDemo.Infrastructure.QueriesImplentation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAuthorPublicationQuery, AuthorPublicationQuery>();

// Add-Migration NewMigration -Project CleanEntityFrameworkDemo.DatabaseMigration
// Update-Database -Project CleanEntityFrameworkDemo.DatabaseMigration
builder.Services.AddDbContext<PublicationContext>(options =>
{
    // https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/projects?tabs=dotnet-core-cli
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PublicationsDatabase"),
        db => db.MigrationsAssembly("CleanEntityFrameworkDemo.DatabaseMigration"));
});

// https://github.com/AutoMapper/AutoMapper.Extensions.Microsoft.DependencyInjection
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(AuthorPublicationQuery)));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/author/{id}/publication", (IAuthorPublicationQuery query,
    int id) => {

        var result = query.GetAuthorPublication(id);
        //return result;

        // https://learn.microsoft.com/en-us/ef/core/querying/related-data/serialization
        JsonSerializerOptions options = new() {
            //ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };
        return JsonSerializer.Serialize(result, options);
    });

app.Run();