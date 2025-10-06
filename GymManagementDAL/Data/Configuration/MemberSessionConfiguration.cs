using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configuration
{
    public class MemberSessionConfiguration : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            builder.Property(MS => MS.CreatedAt).HasColumnName("BookingDate").HasDefaultValueSql("GETDATE()");

            builder.HasKey(MS => new { MS.MemberId, MS.SessionId });
            builder.Ignore(MS => MS.Id);
        }
    }
}
