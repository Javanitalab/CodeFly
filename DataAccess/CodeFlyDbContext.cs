using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models;

public partial class CodeFlyDbContext : DbContext
{
    public CodeFlyDbContext()
    {
    }

    public CodeFlyDbContext(DbContextOptions<CodeFlyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Difficulty> Difficulties { get; set; }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Season> Seasons { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userdetail> Userdetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=codefly;uid=admin;pwd=1221", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Difficulty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("difficulty");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.HasIndex(e => e.SubjectId, "Subject_Id");

            entity.Property(e => e.SubjectId).HasColumnName("Subject_Id");

            entity.HasOne(d => d.Subject).WithMany(p => p.Difficulties)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("difficulty_ibfk_1");
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("feature");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.HasIndex(e => e.RoleId, "Role_Id");

            entity.Property(e => e.RoleId).HasColumnName("Role_Id");

            entity.HasOne(d => d.Role).WithMany(p => p.Features)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("feature_ibfk_1");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("lesson");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.HasIndex(e => e.SeasonId, "Season_Id");

            entity.Property(e => e.FileUrl)
                .HasMaxLength(255)
                .HasColumnName("File_URL");
            entity.Property(e => e.SeasonId).HasColumnName("Season_Id");

            entity.HasOne(d => d.Season).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.SeasonId)
                .HasConstraintName("lesson_ibfk_1");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("permission");

            entity.HasIndex(e => e.FeatureId, "FK_Permission_Feature");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.FeatureId).HasColumnName("Feature_Id");

            entity.HasOne(d => d.Feature).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.FeatureId)
                .HasConstraintName("FK_Permission_Feature");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("role");

            entity.HasIndex(e => e.Name, "Name").IsUnique();
        });

        modelBuilder.Entity<Season>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("season");

            entity.HasIndex(e => e.DifficultyId, "Difficulty_Id");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.DifficultyId).HasColumnName("Difficulty_Id");

            entity.HasOne(d => d.Difficulty).WithMany(p => p.Seasons)
                .HasForeignKey(d => d.DifficultyId)
                .HasConstraintName("season_ibfk_1");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("subject");

            entity.HasIndex(e => e.Name, "Name").IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.HasIndex(e => e.RoleId, "Role_Id");

            entity.HasIndex(e => e.UserDetailId, "UserDetail_Id");

            entity.HasIndex(e => e.Username, "Username").IsUnique();

            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.UserDetailId).HasColumnName("UserDetail_Id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("user_ibfk_2");

            entity.HasOne(d => d.UserDetail).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserDetailId)
                .HasConstraintName("user_ibfk_1");
        });

        modelBuilder.Entity<Userdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("userdetail");

            entity.HasIndex(e => e.UserId, "User_Id");

            entity.Property(e => e.Age).HasMaxLength(255);
            entity.Property(e => e.Bio).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("User_Id");
            entity.Property(e => e.Website).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.Userdetails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("userdetail_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
