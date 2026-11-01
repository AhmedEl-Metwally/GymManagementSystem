namespace GymManagementDAL.Data.Configuration
{
    public class MemberPlanConfiguration : IEntityTypeConfiguration<MemberPlan>
    {
        public void Configure(EntityTypeBuilder<MemberPlan> builder)
        {
            builder.Property(MC => MC.CreatedAt).HasColumnName("StartDate").HasDefaultValueSql("GETDATE()");

            builder.HasKey(MC => new { MC.MemberId, MC.PlanId });
            builder.Ignore(MC => MC.Id);
        }
    }
}
