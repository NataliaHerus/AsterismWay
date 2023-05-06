using AsterismWay.Data.Entities.CategoryEntity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AsterismWay.Data.Entities.FrequencyEntity
{
    public class FrequencyConfiguration : IEntityTypeConfiguration<Frequency>
    {
        public void Configure(EntityTypeBuilder<Frequency> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
             .Property(x => x.Name)
             .IsRequired();
        }

    }
}
