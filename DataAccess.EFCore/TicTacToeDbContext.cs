using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EFCore;

public class TicTacToeDbContext : DbContext
{
    public TicTacToeDbContext(DbContextOptions<TicTacToeDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Game> Games => Set<Game>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=C:\\Users\\thinh.le_ext\\localdb\\sqlite");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Games)
            .WithMany(g => g.Users)
            .UsingEntity(j => j.ToTable("UserGames"));

        modelBuilder.Entity<User>().HasIndex(u => u.Name).IsUnique();
    }
}