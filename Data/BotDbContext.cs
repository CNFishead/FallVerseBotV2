using Microsoft.EntityFrameworkCore;

public class BotDbContext : DbContext
{
  public BotDbContext(DbContextOptions<BotDbContext> options) : base(options) { }

  public DbSet<UserRecord> Users { get; set; }
  public DbSet<UserEconomy> Economies { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<UserEconomy>()
            .HasOne(e => e.User)
            .WithOne(u => u.Economy) // Establish 1-to-1 relationship
            .HasForeignKey<UserEconomy>(e => e.UserId)
            .IsRequired();
  }
}
