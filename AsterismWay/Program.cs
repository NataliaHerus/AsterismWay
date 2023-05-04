using AsterismWay;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IdentityModelEventSource.ShowPII = true;

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

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<IdentityOptions>(options =>
    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(
            builder => builder
                .WithOrigins(
                    "http://localhost:4200",
                    "https://localhost:7010/")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

app.UseForwardedHeaders();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
