using System.Text.Json;
using System.Text.Json.Serialization;
using EfDemo;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PublicationContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("PublicationsDatabase"))
);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/author/{id}/publication", (PublicationContext db, int id) =>
{
    var data = db.Authors.Include(a => a.Articles).ThenInclude(a => a.Authors).FirstOrDefault(a => a.Id == id);

    JsonSerializerOptions options = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = true
    };

    return JsonSerializer.Serialize(data, options);

});

app.Run();
