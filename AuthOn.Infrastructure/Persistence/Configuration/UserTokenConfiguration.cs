using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.Entities.UserTokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthOn.Infrastructure.Persistence.Configuration
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.TokenType)
                .WithMany(t => t.UserTokens)
                .HasForeignKey(u => u.TokenTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.User)
                .WithMany(t => t.UserTokens)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(u => u.UserId).HasConversion(
                userId => userId!.Value,
                value => new UserId(value));

            builder.Property(u => u.Token);

            builder.Property(u => u.IsUsed);

            builder.Property(u => u.IsExpired);

            builder.Property(u => u.RecordCreationMoment);

            builder.Property(u => u.RecordUpdateMoment);
        }
    }
}