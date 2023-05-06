using AsterismWay.Data.Entities;
using AsterismWay.Data.Entities.CategoryEntity;
using AsterismWay.Data.Entities.EventEntity;
using AsterismWay.Data.Entities.FrequencyEntity;
using AsterismWay.Data.Entities.SelectedEventsEntity;
using Microsoft.EntityFrameworkCore;

namespace AsterismWay.Data
{
    public class AsterismWayDbContext: DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<SelectedEvents> SelectedEvents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Frequency> Frequencies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new SelectedEventsConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new FrequencyConfiguration());

        }
        public AsterismWayDbContext(DbContextOptions<AsterismWayDbContext> options)
               : base(options)
        {
        }
    }
}
