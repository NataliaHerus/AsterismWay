using AsterismWay.IdentityServer.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AsterismWay.IdentityServer.Data
{
    public class IdenityServerDbContext : IdentityDbContext<User>
    {
        public IdenityServerDbContext(DbContextOptions<IdenityServerDbContext> options)
            : base(options)
        {
        }
    }
}
