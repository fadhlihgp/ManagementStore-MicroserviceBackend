using AccountAuthMicroservice.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountAuthMicroservice.Config;

public class AppDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<LoginHistory> LoginHistory { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Store> Stores { get; set; }

    protected AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Account>(builder =>
        {
            builder.HasIndex(a => a.Email).IsUnique();
            builder.HasIndex(a => a.NoHp).IsUnique();

        });

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = "1", Name = "SuperAdmin" }
        );
    }
}