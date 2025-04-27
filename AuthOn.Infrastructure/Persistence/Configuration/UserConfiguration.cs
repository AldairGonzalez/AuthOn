using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.ValueObjects;

namespace AuthOn.Infrastructure.Persistence.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).HasConversion(
                userId => userId.Value,
                value => new UserId(value));

            builder.Property(u => u.UserName).HasConversion(
                userName => userName.Value,
                value => UserName.Create(value)!)
                .HasMaxLength(20);

            builder.Property(u => u.Email).HasConversion(
                email => email.Value,
                value => EmailAddress.Create(value)!)
                .HasMaxLength(255);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.EmailConfirmed);

            builder.Property(u => u.IsLocked);

            builder.Property(u => u.HashedPassword);

            builder.Property(u => u.AuthenticationAttempts);

            builder.Property(u => u.DeletionDate);

            builder.Property(u => u.RecordCreationMoment);

            builder.Property(u => u.RecordUpdateMoment);
        }
    }
}