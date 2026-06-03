using AiImageTaskManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AiImageTaskManager.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ImageGenerationTask> ImageGenerationTasks => Set<ImageGenerationTask>();

    public DbSet<GeneratedImage> GeneratedImages => Set<GeneratedImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ImageGenerationTask>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Prompt)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(x => x.NegativePrompt)
                .HasMaxLength(2000);

            entity.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<ApiTestCase>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Method)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(1000);

            entity.HasMany(x => x.Runs)
                .WithOne(x => x.ApiTestCase)
                .HasForeignKey(x => x.ApiTestCaseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ApiTestRun>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ActualResponseBody)
                .HasMaxLength(10000);

            entity.Property(x => x.ErrorMessage)
                .HasMaxLength(2000);
        });
    }

    public DbSet<ApiTestCase> ApiTestCases => Set<ApiTestCase>();

    public DbSet<ApiTestRun> ApiTestRuns => Set<ApiTestRun>();
}