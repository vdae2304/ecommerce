using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Ecommerce.Infrastructure.Data
{
    /// <inheritdoc/>
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;
        private readonly ILoggerFactory? _loggerFactory;

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MediaImage> MediaImages { get; set; }

        /// <inheritdoc/>
        public ApplicationDbContext(IConfiguration config, ILoggerFactory loggerFactory)
        {
            _connectionString = config.GetConnectionString("DefaultConnection") ?? string.Empty;
            _loggerFactory = bool.Parse(config["ConnectionOptions:LogSql"] ?? string.Empty)
                ? loggerFactory : null;
        }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory)
                .UseMySQL(_connectionString)
                .UseSnakeCaseNamingConvention();
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new MediaImageConfiguration());
            modelBuilder.ApplyConfiguration(new ProductAttributeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new ProductImagesConfiguration());
        }

        /// <inheritdoc/>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DateTime now = DateTime.UtcNow;
            foreach (EntityEntry entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedAt = now;
                        entity.UpdatedAt = now;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entity.UpdatedAt = now;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
