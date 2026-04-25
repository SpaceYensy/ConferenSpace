using ConferenSpace.Application.Contracts;
using ConferenSpace.Application.MappingProfiles;
using ConferenSpace.Application.Services;
using ConferenSpace.Infrastructure.Contracts;
using ConferenSpace.Infrastructure.Data;
using ConferenSpace.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=(localdb)\\mssqllocaldb;Database=ConferenSpaceDb;Trusted_Connection=true;";
builder.Services.AddDbContext<ConferenSpaceDbContext>(options =>
    options.UseSqlServer(connectionString,
        x => x.MigrationsAssembly("ConferenSpace.Infrastructure")));

builder.Services.AddAutoMapper(typeof(ConferenSpaceMappingProfile).Assembly);

builder.Services.AddScoped<ISalonRepository, SalonRepository>();
builder.Services.AddScoped<ISolicitanteRepository, SolicitanteRepository>();
builder.Services.AddScoped<IRecursoRepository, RecursoRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();

builder.Services.AddScoped<ISalonService, SalonService>();
builder.Services.AddScoped<ISolicitanteService, SolicitanteService>();
builder.Services.AddScoped<IRecursoService, RecursoService>();
builder.Services.AddScoped<IReservaService, ReservaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ConferenSpaceDbContext>();
    dbContext.Database.Migrate();
}

app.Run();