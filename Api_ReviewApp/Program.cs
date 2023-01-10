using Api_ReviewApp.Data;
using Api_ReviewApp.Interfaces;
using Api_ReviewApp.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//seed data at the beginning
builder.Services.AddTransient<Seed>();

//automapper pack presented with the help of automapper dependency injection pack installed
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//injection for interfaces
builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//use sql for database and here is connection string for it
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
}
);


var app = builder.Build();

//middleware here

//seed before app starts.. call on terminal dotnet run seeddata
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var scopedService = scope.ServiceProvider.GetService<Seed>();
        scopedService.SeedDataContext();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
