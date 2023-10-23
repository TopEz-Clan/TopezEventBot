using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Entities;

namespace TopezEventBot.Data.Context;

public class TopezContext : DbContext
{
   public DbSet<AccountLink> AccountLinks { get; set; }
   public DbSet<Event> Events { get; set; }

   public TopezContext(DbContextOptions options) : base(options)
   {
   }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
    modelBuilder.Entity<Event>()
        .HasMany(e => e.Participants)
        .WithMany(e => e.Events)
        .UsingEntity<EventParticipation>();
   }

   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite($"Data Source={Util.DbPath()}");
}