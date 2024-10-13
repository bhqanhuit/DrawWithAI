using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace DrawApi.Models;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GeneratedImage> GeneratedImages { get; set; }

    public virtual DbSet<Sketch> Sketches { get; set; }

    public virtual DbSet<SketchToImageView> SketchToImageViews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3100;database=SketchToImage;user id=root;password=MatKhau123!", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.0.1-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<GeneratedImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PRIMARY");

            entity.ToTable("GeneratedImage");

            entity.HasIndex(e => e.ImageName, "ImageName").IsUnique();

            entity.HasIndex(e => e.SketchId, "SketchId");

            entity.HasOne(d => d.Sketch).WithMany(p => p.GeneratedImages)
                .HasForeignKey(d => d.SketchId)
                .HasConstraintName("GeneratedImage_ibfk_1");
        });

        modelBuilder.Entity<Sketch>(entity =>
        {
            entity.HasKey(e => e.SketchId).HasName("PRIMARY");

            entity.HasIndex(e => e.SketchName, "SketchName").IsUnique();

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Prompt).HasMaxLength(255);
            entity.Property(e => e.UploadAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.User).WithMany(p => p.Sketches)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Sketches_ibfk_1");
        });

        modelBuilder.Entity<SketchToImageView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SketchToImageView");

            entity.Property(e => e.ImageName).HasMaxLength(255);
            entity.Property(e => e.Prompt).HasMaxLength(255);
            entity.Property(e => e.SketchName).HasMaxLength(255);
            entity.Property(e => e.UploadAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
