﻿using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public partial class CodeFlyDbContext : DbContext
{
    public CodeFlyDbContext()
    {
    }

    public CodeFlyDbContext(DbContextOptions<CodeFlyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chapter> Chapters { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Quest> Quests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userdetail> Userdetails { get; set; }

    public virtual DbSet<Userlesson> Userlessons { get; set; }

    public virtual DbSet<Userquest> Userquests { get; set; }

    public virtual DbSet<UserquestUserlesson> UserquestUserlessons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=38.242.149.105;Port=5432;Database=codefly;User Id=admin;Password=verysecret;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chapter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("chapter_pkey");

            entity.ToTable("chapter");

            entity.HasIndex(e => e.Name, "chapter_name_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('season_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");

            entity.HasOne(d => d.Subject).WithMany(p => p.Chapters)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("chapter_subject");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lesson_pkey");

            entity.ToTable("lesson");

            entity.HasIndex(e => e.Name, "lesson_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChapterId).HasColumnName("chapter_id");
            entity.Property(e => e.FileUrl)
                .HasMaxLength(255)
                .HasColumnName("file_url");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Chapter).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.ChapterId)
                .HasConstraintName("lesson_chapter_id_fkey");
        });

        modelBuilder.Entity<Quest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_pkey");

            entity.ToTable("quest");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Completed).HasColumnName("completed");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.NeededProgress)
                .HasColumnType("bit(1)")
                .HasColumnName("needed_progress");
            entity.Property(e => e.RewardType)
                .HasColumnType("bit(1)")
                .HasColumnName("reward_type");
            entity.Property(e => e.RewardValue).HasColumnName("reward_value");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.HasIndex(e => e.Name, "role_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("subject_pkey");

            entity.ToTable("subject");

            entity.HasIndex(e => e.Name, "subject_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "user_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "user_username_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Coins).HasColumnName("coins");
            entity.Property(e => e.Cups).HasColumnName("cups");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UserdetailId).HasColumnName("userdetail_id");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("user_role_id_fkey");

            entity.HasOne(d => d.Userdetail).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserdetailId)
                .HasConstraintName("user_userdetail_id_fkey");
        });

        modelBuilder.Entity<Userdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("userdetail_pkey");

            entity.ToTable("userdetail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Age)
                .HasMaxLength(255)
                .HasColumnName("age");
            entity.Property(e => e.Bio)
                .HasMaxLength(255)
                .HasColumnName("bio");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Website)
                .HasMaxLength(255)
                .HasColumnName("website");
        });

        modelBuilder.Entity<Userlesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("userlesson_pkey");

            entity.ToTable("userlesson");

            entity.HasIndex(e => e.Id, "userlesson_id_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Userlessons)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userlesson_lesson");

            entity.HasOne(d => d.User).WithMany(p => p.Userlessons)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userlesson_user");
        });

        modelBuilder.Entity<Userquest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("userquest_pkey");

            entity.ToTable("userquest");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Creationdate).HasColumnName("creationdate");
            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.QuestId).HasColumnName("quest_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Userquests)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("userquest_lesson");

            entity.HasOne(d => d.Quest).WithMany(p => p.Userquests)
                .HasForeignKey(d => d.QuestId)
                .HasConstraintName("quest_user");

            entity.HasOne(d => d.User).WithMany(p => p.Userquests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_quest");
        });

        modelBuilder.Entity<UserquestUserlesson>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.UserquestId, e.UserlessonId }).HasName("userquest_userlesson_pkey");

            entity.ToTable("userquest_userlesson");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserquestId).HasColumnName("userquest_id");
            entity.Property(e => e.UserlessonId).HasColumnName("userlesson_id");

            entity.HasOne(d => d.Userlesson).WithMany(p => p.UserquestUserlessons)
                .HasForeignKey(d => d.UserlessonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uqul_userlesson");

            entity.HasOne(d => d.Userquest).WithMany(p => p.UserquestUserlessons)
                .HasForeignKey(d => d.UserquestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uqul_userquest");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
