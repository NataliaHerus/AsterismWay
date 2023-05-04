using System.ComponentModel.DataAnnotations;
using static AsterismWay.IdentityServer.Validators.IdentityValidation;

namespace AsterismWay.IdentityServer.Models.Identity
{
    public class LoginRequestModel
    {
        [Required]
        [Email]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
