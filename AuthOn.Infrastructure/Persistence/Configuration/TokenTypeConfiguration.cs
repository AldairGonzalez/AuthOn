using AuthOn.Domain.Entities.TokenTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthOn.Infrastructure.Persistence.Configuration
{
    public class TokenTypeConfiguration: IEntityTypeConfiguration<TokenType>
    {
        public void Configure(EntityTypeBuilder<TokenType> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.ExpirationTimeInHours)
                .IsRequired();

            builder.HasData(
                TokenType.AccessToken,
                TokenType.ActivationToken,
                TokenType.RefreshToken
            );
        }
    }
}