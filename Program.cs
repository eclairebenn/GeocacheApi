using Microsoft.EntityFrameworkCore;
using GeocacheAPI.Data;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

    
builder.Services.AddDbContext<GeocacheContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("GeocacheContextDb")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Geocache API",
        Description = ".NET 6 Web API for managing Geocaches and Items",

    });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//Add Geocaching Repository Interface as a Service
builder.Services.AddScoped<IGeocacheRepository, GeocacheRepository>();
var app = builder.Build();

//Seed Data Initializer
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    GeocacheSeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
