using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;



namespace GymManagementDAL.Data.Context
{
    public class GymDbContext(DbContextOptions<GymDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<ApplicationUser>
                (
                    entities => 
                    {
                        entities.Property(E => E.FirstName)
                                .HasColumnType("varchar")
                                .HasMaxLength(60);

                        entities.Property(E => E.LastName)
                                .HasColumnType("varchar")
                                .HasMaxLength(60);
                    }
                );
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberPlan> MemberPlans{ get; set; }
        public DbSet<MemberSession> MemberSessions{ get; set; }
        public DbSet<Plan> Plans{ get; set; }
        public DbSet<Session> Sessions{ get; set; }
        public DbSet<Trainer> Trainers{ get; set; }
    }
}
 