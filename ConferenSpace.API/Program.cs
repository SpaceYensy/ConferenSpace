using ConferenSpace.Application.Contracts;
using ConferenSpace.Application.MappingProfiles;
using ConferenSpace.Application.Services;
using ConferenSpace.Infrastructure.Contracts;
using ConferenSpace.Infrastructure.Data;
using ConferenSpace.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configurar DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=(localdb)\\mssqllocaldb;Database=ConferenSpaceDb;Trusted_Connection=true;";
builder.Services.AddDbContext<ConferenSpaceDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registrar AutoMapper
builder.Services.AddAutoMapper(typeof(ConferenSpaceMappingProfile).Assembly);

// Registrar Repositorios
builder.Services.AddScoped<ISalonRepository, SalonRepository>();
builder.Services.AddScoped<ISolicitanteRepository, SolicitanteRepository>();
builder.Services.AddScoped<IRecursoRepository, RecursoRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();

// Registrar Servicios
builder.Services.AddScoped<ISalonService, SalonService>();
builder.Services.AddScoped<ISolicitanteService, SolicitanteService>();
builder.Services.AddScoped<IRecursoService, RecursoService>();
builder.Services.AddScoped<IReservaService, ReservaService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Crear la base de datos si no existe (ejecutar migraciones)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ConferenSpaceDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
