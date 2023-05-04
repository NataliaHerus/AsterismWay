﻿using Microsoft.AspNetCore.Identity;

namespace AsterismWay.IdentityServer.Data.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
