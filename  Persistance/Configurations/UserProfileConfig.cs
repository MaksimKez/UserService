using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class UserProfileConfig : IEntityTypeConfiguration<UserProfileEntity>
{
    public void Configure(EntityTypeBuilder<UserProfileEntity> builder)
    {
        builder.ToTable("user_profiles");

        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.PreferredLanguage);
        builder.HasIndex(p => p.LastNotifiedAt);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(300);

        builder.Property(p => p.PreferredLanguage)
            .HasMaxLength(10);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.LastNotifiedAt);

        builder.HasOne(u => u.Filter)
            .WithOne(f => f.Profile)
            .HasForeignKey<UserFilterEntity>(f => f.ProfileId);
    }
}
