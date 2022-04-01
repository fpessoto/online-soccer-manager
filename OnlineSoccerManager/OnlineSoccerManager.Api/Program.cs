using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineSoccerManager.Api.Config;
using OnlineSoccerManager.Application.Commands;
using OnlineSoccerManager.Domain.Builders;
using OnlineSoccerManager.Domain.Interfaces;
using OnlineSoccerManager.Domain.Players;
using OnlineSoccerManager.Domain.Teams;
using OnlineSoccerManager.Domain.Transfers;
using OnlineSoccerManager.Domain.Users;
using OnlineSoccerManager.Infrastructure.DatabaseEFCore.Context;
using OnlineSoccerManager.Infrastructure.DatabaseEFCore.Players;
using OnlineSoccerManager.Infrastructure.DatabaseEFCore.Teams;
using OnlineSoccerManager.Infrastructure.DatabaseEFCore.Transfers;
using OnlineSoccerManager.Infrastructure.DatabaseEFCore.Users;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void RegisterServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var connectionString = builder.Configuration.GetConnectionString("AppDb");
    builder.Services.AddDbContext<OnlineSoccerDBContext, OnlineSoccerDBContext>(opt => opt.UseSqlServer(connectionString));


    builder.Services.AddDbContext<OnlineSoccerDBContext>(x => x.UseSqlServer(connectionString));

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    builder.Services.AddScoped<ITeamBuilder, TeamBuilder>();
    builder.Services.AddScoped<IPlayerBuilder, PlayerBuilder>();

    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
    builder.Services.AddScoped<ITeamRepository, TeamRepository>();
    builder.Services.AddScoped<ITransferRepository, TransferRepository>();

    builder.Services.AddScoped<ICommand<UserSignupCommand, User>, UserSignupCommandHandler>();
    builder.Services.AddScoped<ICommand<CreateTeamCommand, Team>, CreateTeamCommandHandler>();
    builder.Services.AddScoped<ICommand<UpdateTeamCommand, Team>, UpdateTeamCommandHandler>();
    builder.Services.AddScoped<ICommand<RemovePlayerFromTransferListCommand, Player>, RemovePlayerFromTransferListCommandHandler>();
    builder.Services.AddScoped<ICommand<SetPlayerToTransferListCommand, Player>, SetPlayerToTransferListCommandHandler>();
    builder.Services.AddScoped<ICommand<UpdatePlayerCommand, Player>, UpdatePlayerCommandHandler>();
    builder.Services.AddScoped<ICommand<TransferPlayerCommand, Transfer>, TransferPlayerCommandHandler>();

    var key = Encoding.ASCII.GetBytes(Settings.Secret);
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
}