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


