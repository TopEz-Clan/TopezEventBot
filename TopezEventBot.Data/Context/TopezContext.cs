using Microsoft.EntityFrameworkCore;
using TopezEventBot.Data.Model;

namespace TopezEventBot.Data.Context;

public class TopezContext : DbContext
{
   public DbSet<AccountLink> AccountLinks { get; set; }

   public TopezContext(DbContextOptions options) : base(options)
   {
   }

   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite($"Data Source={Util.DbPath()}");
}