using BookStoreAPI.Repository;
using BookStoreAPI.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Legg til databaseforbindelse
builder.Services.AddScoped<BookRepository>(sp => new BookRepository(
    builder.Configuration.GetConnectionString("DefaultConnection")!
));

var app = builder.Build();

// Map API-endepunktene
app.MapBookEndpoints();

// Kjør applikasjonen
app.Run();
