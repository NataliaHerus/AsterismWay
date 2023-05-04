using AsterismWay.IdentityServer.Data.Models;
using AsterismWay.IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using AsterismWay.IdentityServer;
using System.Text;
using AsterismWay.IdentityServer.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IdenityServerDbContext>(options =>
    options.UseSqlServer(connectionString!));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<IdenityServerDbContext>().AddDefaultTokenProviders();

var applicationSettingsConfiguration = builder.Configuration.GetSection("ApplicationSettings");
builder.Services.Configure<AppSettings>(applicationSettingsConfiguration);

var appSettings = applicationSettingsConfiguration.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

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
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors(options => options
 .WithOrigins("http://localhost:4200/",
              "https://localhost:7179/")
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.ApplyMigrations();
app.Run();
