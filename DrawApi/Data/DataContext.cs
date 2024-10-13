using Microsoft.EntityFrameworkCore;
using DrawApi.Models;

namespace DrawApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Sketch> Sketches { get; set; }
        public DbSet<GeneratedImage> GeneratedImages { get; set; }

        // Add a DbSet for the view
        public DbSet<SketchToImageView> SketchToImageView { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map the view to the SketchToImageView model
            modelBuilder.Entity<SketchToImageView>().HasNoKey().ToView("SketchToImageView");
        }
    }
}

/* Auto - generated data context */
//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder
//            .UseCollation("utf8mb4_0900_ai_ci")
//            .HasCharSet("utf8mb4");

//        modelBuilder.Entity<GeneratedImage>(entity =>
//        {
//            entity.HasKey(e => e.ImageId).HasName("PRIMARY");

//            entity.ToTable("GeneratedImage");

//            entity.HasIndex(e => e.ImageName, "ImageName").IsUnique();

//            entity.HasIndex(e => e.SketchId, "SketchId");

//            entity.HasOne(d => d.Sketch).WithMany(p => p.GeneratedImages)
//                .HasForeignKey(d => d.SketchId)
//                .HasConstraintName("GeneratedImage_ibfk_1");
//        });

//        modelBuilder.Entity<Sketch>(entity =>
//        {
//            entity.HasKey(e => e.SketchId).HasName("PRIMARY");

//            entity.HasIndex(e => e.SketchName, "SketchName").IsUnique();

//            entity.HasIndex(e => e.UserId, "UserId");

//            entity.Property(e => e.Prompt).HasMaxLength(255);
//            entity.Property(e => e.UploadAt)
//                .HasDefaultValueSql("CURRENT_TIMESTAMP")
//                .HasColumnType("timestamp");

//            entity.HasOne(d => d.User).WithMany(p => p.Sketches)
//                .HasForeignKey(d => d.UserId)
//                .HasConstraintName("Sketches_ibfk_1");
//        });

//        modelBuilder.Entity<SketchToImageView>(entity =>
//        {
//            entity
//                .HasNoKey()
//                .ToView("SketchToImageView");

//            entity.Property(e => e.ImageName).HasMaxLength(255);
//            entity.Property(e => e.Prompt).HasMaxLength(255);
//            entity.Property(e => e.SketchName).HasMaxLength(255);
//            entity.Property(e => e.UploadAt)
//                .HasDefaultValueSql("CURRENT_TIMESTAMP")
//                .HasColumnType("timestamp");
//        });

//        modelBuilder.Entity<User>(entity =>
//        {
//            entity.HasKey(e => e.UserId).HasName("PRIMARY");

//            entity.Property(e => e.CreatedAt)
//                .HasDefaultValueSql("CURRENT_TIMESTAMP")
//                .HasColumnType("timestamp");
//            entity.Property(e => e.Email).HasMaxLength(100);
//            entity.Property(e => e.Password).HasMaxLength(50);
//            entity.Property(e => e.Username).HasMaxLength(50);
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
