using AuthOn.Domain.Entities.EmailStates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthOn.Infrastructure.Persistence.Configuration
{
    public class EmailStateConfiguration : IEntityTypeConfiguration<EmailState>
    {
        public void Configure(EntityTypeBuilder<EmailState> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasData(
                EmailState.Pending,
                EmailState.Sent,
                EmailState.Failed
            );
        }
    }
}