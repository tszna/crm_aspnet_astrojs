using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Data.Configurations;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TimeSession> TimeSessions { get; set; }
        public DbSet<Absence> Absences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply seed configuration
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                
                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20) unsigned")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(191);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(191);

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(191);

                entity.Property(e => e.RememberToken)
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<TimeSession>(entity =>
            {
                entity.ToTable("time_sessions");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20) unsigned")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.UserId)
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.StartTime)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.EndTime)
                    .HasColumnType("timestamp")
                    .IsRequired(false);

                entity.Property(e => e.ElapsedTime)
                    .HasColumnType("time")
                    .IsRequired(false);

                entity.Property(e => e.FullElapsedTime)
                    .HasColumnType("time")
                    .IsRequired(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Absence>(entity =>
            {
                entity.ToTable("absences");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20) unsigned")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.UserId)
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .IsRequired();

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .IsRequired();

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
