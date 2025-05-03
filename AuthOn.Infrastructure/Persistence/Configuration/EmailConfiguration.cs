using AuthOn.Domain.Entities.Emails;
using AuthOn.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthOn.Infrastructure.Persistence.Configuration
{
    public class EmailConfiguration : IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.DestinationEmail).HasConversion(
                email => email!.Value,
                value => EmailAddress.Create(value)!)
                .HasMaxLength(255);

            builder.Property(u => u.Subject);

            builder.Property(u => u.Message);

            builder.HasOne(r => r.EmailState)
                .WithMany(e => e.Emails)
                .HasForeignKey(r => r.EmailStateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(u => u.Visualized);

            builder.Property(u => u.RecordCreationMoment);

            builder.Property(u => u.RecordUpdateMoment);
        }
    }
}