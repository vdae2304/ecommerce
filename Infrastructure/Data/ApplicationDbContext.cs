using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Ecommerce.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _config;
        private readonly ILoggerFactory? _loggerFactory;

        public ApplicationDbContext(IConfiguration config, ILoggerFactory loggerFactory)
        {
            _config = config;
            _loggerFactory = bool.Parse(config["ConnectionOptions:LogSql"] ?? string.Empty)
                ? loggerFactory : null;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory)
                .UseMySQL(_config.GetConnectionString("DefaultConnection") ?? string.Empty)
                .UseSnakeCaseNamingConvention();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new CategoryConfiguration().Configure(modelBuilder.Entity<Category>());
            new ProductConfiguration().Configure(modelBuilder.Entity<Product>());
            new ImageConfiguration().Configure(modelBuilder.Entity<Image>());
            new ProductAttributeConfiguration().Configure(modelBuilder.Entity<ProductAttribute>());
            new ProductCategoriesConfiguration().Configure(modelBuilder.Entity<ProductCategories>());
            new ProductImagesConfiguration().Configure(modelBuilder.Entity<ProductImages>());
            new MeasureUnitConfiguration().Configure(modelBuilder.Entity<MeasureUnit>());
        }

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
