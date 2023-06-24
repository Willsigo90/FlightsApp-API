using BusinessLayer;
using BusinessLayer.Implementation;
using BusinessLayer.Interfaz;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory());
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    });

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
    //.AddFluentValidation(c =>
    //{
    //    c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    //});
    

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddScoped<IRoutes, Routes>();

builder.Services.AddTransient<IServiceFlights, ServiceFlights>();

//builder.Services.AddScoped<IFlightBuilder, FlightBuilder>();
builder.Services.AddScoped<IGraphBuilder, GraphBuilder>();
builder.Services.AddScoped<IShortestRouteFinder, ShortestRouteFinder>();

builder.Services.AddTransient<IServiceCurrency, ServiceCurrency>();





var app = builder.Build();

app.UseCors(options =>
{
    options.WithOrigins("http://localhost:4200");
    options.AllowAnyMethod();
    options.AllowAnyHeader();
});



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
