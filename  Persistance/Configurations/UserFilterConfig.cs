using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class UserFilterConfig : IEntityTypeConfiguration<UserFilterEntity>
{
    public void Configure(EntityTypeBuilder<UserFilterEntity> builder)
    {
        builder.ToTable("user_filters");

        builder.HasKey(f => f.Id);
        builder.HasIndex(f => f.ProfileId);

        builder.HasOne(f => f.Profile)
            .WithOne(p => p.Filter)
            .HasForeignKey<UserFilterEntity>(f => f.ProfileId);
        
        builder.Property(f => f.MinPrice).HasPrecision(10, 2);
        builder.Property(f => f.MaxPrice).HasPrecision(10, 2);

        builder.Property(f => f.MinAreaMeterSqr).HasPrecision(10, 2);
        builder.Property(f => f.MaxAreaMeterSqr).HasPrecision(10, 2);

        builder.Property(f => f.MinRooms);
        builder.Property(f => f.MaxRooms);
        builder.Property(f => f.MinFloor);
        builder.Property(f => f.MaxFloor);
        builder.Property(f => f.NewerThanDays);

        builder.Property(f => f.IsFurnished).HasDefaultValue(false);
        builder.Property(f => f.PetsAllowed).HasDefaultValue(false);
        builder.Property(f => f.HasBalcony).HasDefaultValue(false);
        builder.Property(f => f.HasAppliances).HasDefaultValue(false);
    }
}
