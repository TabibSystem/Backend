using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace TabibApp.Infrastructure.Configration
{
    public class SpecializationConfigartion : IEntityTypeConfiguration<Specialization>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Specialization> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(p=> p.Name).HasAnnotation("MaxLength", 50).IsRequired();
           

        }
    }
}
