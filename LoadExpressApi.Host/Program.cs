using LoadExpressApi.Domain.Entities;
using LoadExpressApi.Host;
using LoadExpressApi.Persistence;
using LoadExpressApi.Persistence.Context;
using LoadExpressApi.Persistence.Logging.Serilog;
using Microsoft.AspNetCore.Identity;
using LoadExpressApi.Application.Abstraction.Repositories;
using LoadExpressApi.Application.Interfaces;
using LoadExpressApi.Application.Services;
using LoadExpressApi.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterSerilog();

builder.Services
            .AddInfrastructure(builder.Configuration)
            .RegisterJWT(builder.Configuration)
            .RegisterSwagger().
             AddIdentityApiEndpoints<User>()
 .AddRoles<Role>()
 .AddEntityFrameworkStores<ApplicationDbContext>()
 .AddDefaultTokenProviders(); ;
// Add services to the container.
// Explicit dependency registrations
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseInfrastructure(builder.Configuration);
app.MapControllers();

app.Run();
