using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configuration
{
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(GC => GC.Name).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(GC => GC.Email).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(GC => GC.Phone).HasColumnType("varchar").HasMaxLength(11);

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUserVaildEmailCheck", "Email Like '_%@_%._%'");
                Tb.HasCheckConstraint("GymUserVaildPhoneCheck", "Phone Like '01%' and phone not like '%[^0-9]%'");
            });

            builder.HasIndex(GC =>GC.Email).IsUnique();
            builder.HasIndex(GC =>GC.Phone).IsUnique();

            builder.OwnsOne(GU =>GU.Address, AddressBuilder => 
            {
                AddressBuilder.Property(s =>s.Street).HasColumnName("Street").HasColumnType("varchar").HasMaxLength(30);
                AddressBuilder.Property(c =>c.City).HasColumnName("City").HasColumnType("varchar").HasMaxLength(30);
                AddressBuilder.Property(BN =>BN.BuildingNumber).HasColumnName("BuildingNumber");
            });
        }
    }
}
