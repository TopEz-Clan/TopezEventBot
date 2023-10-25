using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Entities;

namespace TopezEventBot.Data.Context;

public class TopezContext : DbContext
{
   public DbSet<AccountLink> AccountLinks { get; set; }
   public DbSet<TrackableEvent> TrackableEvents { get; set; }
   
   public DbSet<SchedulableEvent> SchedulableEvents { get; set; }
   public DbSet<GuildWarningChannel> GuildWarningChannels { get; set; }
   public DbSet<Warning> Warnings { get; set; }
   
   public TopezContext(DbContextOptions options) : base(options)
   {
   }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       
    modelBuilder.Entity<TrackableEvent>()
        .HasMany(e => e.Participants)
        .WithMany(e => e.TrackableEvents)
        .UsingEntity<TrackableEventParticipation>();
    
    modelBuilder.Entity<SchedulableEvent>()
        .HasMany(e => e.Participants)
        .WithMany(e => e.SchedulableEvents)
        .UsingEntity<SchedulableEventParticipation>();
   }

   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite($"Data Source={Util.DbPath()}");
}