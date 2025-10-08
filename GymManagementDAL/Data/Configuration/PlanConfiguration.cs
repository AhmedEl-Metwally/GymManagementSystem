using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configuration
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(PC =>PC.Name).HasColumnType("Varchar").HasMaxLength(50);
            builder.Property(PC =>PC.Description).HasColumnType("Varchar").HasMaxLength(200);
            builder.Property(PC =>PC.Price).HasPrecision(10,2);

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("PlandurationCheck", "DurationDays BETWEEN 1 AND 365");
            });
        }
    }
}
