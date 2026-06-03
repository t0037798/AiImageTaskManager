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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(x => x.Email)
                .IsUnique();

            entity.Property(x => x.DisplayName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            entity.HasMany(x => x.ImageGenerationTasks)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.ApiTestCases)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public DbSet<ApiTestCase> ApiTestCases => Set<ApiTestCase>();

    public DbSet<ApiTestRun> ApiTestRuns => Set<ApiTestRun>();

    public DbSet<User> Users => Set<User>();
}