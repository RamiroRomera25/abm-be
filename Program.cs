using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Domain;
using technical_tests_backend_ssr.Repositories;
using technical_tests_backend_ssr.Repositories.Impl;
using technical_tests_backend_ssr.Services.Impl;
using technical_tests_backend_ssr.Services.Interface;
using technical_tests_backend_ssr.Services.MovementStrategy;
using technical_tests_backend_ssr.Services.MovementStrategy.Impl;
using technical_tests_backend_ssr.Services.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//configurar Entity Framework Core con MySQL
string connectionString = "Server=localhost;Port=3306;Database=prueba-be;Uid=rami;Pwd=rami;";
builder.Services.AddDbContext<TechnicalTestDbContext>(opt => 
    opt.UseMySql(connectionString,
        new MySqlServerVersion(ServerVersion.AutoDetect(connectionString))
    )
);

//configurar automapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IMovementFactoryHandler, MovementFactoryHandler>();
builder.Services.AddScoped<MovementBidHandler>();
builder.Services.AddScoped<MovementContributionHandler>();

builder.Services.AddScoped<IMovementRepository, MovementRepository>();
builder.Services.AddScoped<IMovementService, MovementService>();

builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();

builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<IAuctionService, AuctionService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<AuctionDtoPostValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AuctionDtoPutValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MovementDtoPostValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PurchaseDtoPostValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PurchaseDtoPutValidator>();

var app = builder.Build();

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
