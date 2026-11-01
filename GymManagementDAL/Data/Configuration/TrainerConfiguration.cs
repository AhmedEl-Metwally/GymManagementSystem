namespace GymManagementDAL.Data.Configuration
{
    public class TrainerConfiguration : GymUserConfiguration<Trainer>, IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(CA => CA.CreatedAt).HasColumnName("HireDate").HasDefaultValueSql("GETDATE()");
            base.Configure(builder);
        }
    }
}
