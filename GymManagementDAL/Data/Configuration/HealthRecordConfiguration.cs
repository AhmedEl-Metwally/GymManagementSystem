namespace GymManagementDAL.Data.Configuration
{
    public class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members");
            builder.HasOne<Member>().WithOne(M => M.HealthRecord).HasForeignKey<HealthRecord>(H => H.Id);
            builder.Ignore(H => H.CreatedAt);
        }
    }
}
