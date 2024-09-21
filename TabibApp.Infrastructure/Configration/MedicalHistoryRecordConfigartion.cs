using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TabibApp.Infrastructure.Configration
{
    public class MedicalHistoryRecordConfigartion : IEntityTypeConfiguration<MedicalHistoryRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalHistoryRecord> builder)
        {
            builder.Property(m => m.FileName).IsRequired().HasMaxLength(255);
            builder.Property(m => m.FileUrl).IsRequired();
            builder.Property(m => m.UploadDate).IsRequired();

            builder.HasOne(m => m.Patient)
                   .WithMany(p => p.MedicalHistory)
                   .HasForeignKey(m => m.PatientId);
        }
    }
}
