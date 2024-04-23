using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using PWManager.Data.Models;
using PWManager.Domain.Entities;

namespace PWManager.Data.Persistance; 

internal class ApplicationDbContext : DbContext {

    public DbSet<AccountModel> Accounts { get; set; }
    public DbSet<GroupModel> Groups { get; set; }
    public DbSet<SettingsModel> Settings { get; set; }
    public DbSet<UserModel> Users { get; set; }

    
    public string Path { get; private set; }
    
    public ApplicationDbContext(string dbPath) : base(CreateOptionsBuilder(dbPath)) {
        Path = dbPath;
    }

    private static DbContextOptions CreateOptionsBuilder(string path) {
        var builder = new DbContextOptionsBuilder(); 
        builder.UseSqlite("Data Source=" + path);
        return builder.Options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<AccountModel>()
            .HasOne<GroupModel>()
            .WithMany(e => e.Accounts)
            .HasForeignKey(e => e.GroupId);

        modelBuilder.Entity<GroupModel>()
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId);

        modelBuilder.Entity<SettingsModel>()
            .HasOne(e => e.User)
            .WithOne()
            .HasForeignKey<SettingsModel>(e => e.UserId);
    }
}