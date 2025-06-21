using HomeTask1.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace HomeTask1.Users.Infrastructure;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }
    
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subscription>()
            .Property(s => s.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Subscription>()
            .HasMany(s => s.Users)
            .WithOne(u => u.Subscription)
            .HasForeignKey(u => u.SubscriptionId);
    }

}
