using AsterismWay.IdentityServer.Data.Models;
using AsterismWay.IdentityServer.Models.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AsterismWay.IdentityServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly AppSettings appSettings;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public IdentityController(UserManager<User> userManager, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
            };

            var result = await this.userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Roles.User.ToString());
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpGet]
        [Route("all")]
        public async Task<List<User>> GetUsers()
        {
            return await Task.Run(() =>
            {
                return userManager.Users.ToList();
            });
        }

        [HttpGet]
        [Route("user")]
        public async Task<ActionResult> GetCurrentUser()
        {
            string userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            var result = await userManager.FindByIdAsync(userId);
            return Ok(result);
        }


        [HttpGet]
        [Route("role")]
        public async Task<ActionResult> GetCurrentRole()
        {
            string userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            var result = await userManager.FindByIdAsync(userId);
            var a = userManager.GetRolesAsync(result).Result.FirstOrDefault();
            if (a == null)
            {
                return Ok(null);
            }
            return Ok(new { role = a }); 
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login(LoginRequestModel model)
        {
            var user = await this.userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized("Користувача з вказаною електронною поштою не існує");
            }

            var passwordValid = await this.userManager.CheckPasswordAsync(user, model.Password);

            if (!passwordValid)
            {
                return Unauthorized("Неправильний пароль");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = encryptedToken
            });
        }

        [HttpPost]
        [Route("logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }
    }
}
