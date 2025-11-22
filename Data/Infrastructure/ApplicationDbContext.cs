using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Trick> Tricks { get; set; }
    public DbSet<PrerequisiteProgressionRelation> PrerequisiteProgressionRelations { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<TrickCategory> TrickCategories { get; set; }
    public DbSet<Difficulty> Difficulties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TrickCategory>()
            .HasKey(tc => new { tc.TrickId, tc.CategoryId });
        modelBuilder.Entity<PrerequisiteProgressionRelation>()
            .HasKey(ppr => new { ppr.PrerequisiteId, ppr.ProgressionId });

        modelBuilder.Entity<PrerequisiteProgressionRelation>()
            .HasOne(p => p.Prerequisite)
            .WithMany(p => p.Progressions)
            .HasForeignKey(p => p.PrerequisiteId);

        modelBuilder.Entity<PrerequisiteProgressionRelation>()
            .HasOne(p => p.Progression)
            .WithMany(p => p.Prerequisites)
            .HasForeignKey(p => p.ProgressionId);
    }
};