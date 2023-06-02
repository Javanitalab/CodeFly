using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

public class CodeFlyDbContext : DbContext
{
    public CodeFlyDbContext(DbContextOptions<CodeFlyDbContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Feature> Feature { get; set; }
    public DbSet<Permission> Permission { get; set; }
    public DbSet<UserDetail> UserDetail { get; set; }
    public DbSet<Subject> Subject { get; set; }
    public DbSet<Difficulty> Difficulty { get; set; }
    public DbSet<Season> Season { get; set; }
    public DbSet<Lesson> Lesson { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasOne(u => u.UserDetail)
            .WithOne(ud => ud.User)
            .HasForeignKey<UserDetail>(ud => ud.User_Id);

        modelBuilder.Entity<Feature>()
            .HasOne(f => f.Role)
            .WithMany(r => r.Features)
            .HasForeignKey(f => f.Role_Id);

        modelBuilder.Entity<Feature>()
            .HasOne(f => f.Permission)
            .WithMany(p => p.Features)
            .HasForeignKey(f => f.Permission_Id);

        modelBuilder.Entity<Difficulty>()
            .HasOne(d => d.Subject)
            .WithMany(s => s.Difficulties)
            .HasForeignKey(d => d.Subject_Id);

        modelBuilder.Entity<Season>()
            .HasOne(s => s.Difficulty)
            .WithMany(d => d.Seasons)
            .HasForeignKey(s => s.Difficulty_Id);

        modelBuilder.Entity<Lesson>()
            .HasOne(l => l.Season)
            .WithMany(s => s.Lessons)
            .HasForeignKey(l => l.Season_Id);
    }
}