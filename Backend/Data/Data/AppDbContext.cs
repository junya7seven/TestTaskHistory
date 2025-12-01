using Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<History> Histories { get; set; } = null;
        public DbSet<EventType> EventTypes { get; set; } = null;
        public DbSet<User> Users { get; set; } = null;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<History>(entity =>
            {
                entity.ToTable("Histories");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Text)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(e => e.DateTime)
                    .HasDefaultValueSql("NOW()");

                entity.HasOne(h => h.User)
                    .WithMany(u => u.Histories)
                    .HasForeignKey(h => h.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(h => h.EventType)
                    .WithMany(e => e.Histories)
                    .HasForeignKey(h => h.EventTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<EventType>(entity =>
            {
                entity.ToTable("EventTypes");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.FullName)
                    .IsRequired();
            });
        }

    }
}
