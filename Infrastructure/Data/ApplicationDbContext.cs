﻿using Ecommerce.Common.Interfaces;
using Ecommerce.Common.Models.IAM;
using Ecommerce.Common.Models.Schema;
using Ecommerce.Infrastructure.Configuration.IAM;
using Ecommerce.Infrastructure.Configuration.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Ecommerce.Infrastructure.Data
{
    /// <inheritdoc/>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<MediaImage> MediaImages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductCategories> ProductCategories { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }

        /// <inheritdoc/>
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Schema
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new MediaImageConfiguration());
            modelBuilder.ApplyConfiguration(new ProductAttributeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new ProductImagesConfiguration());

            // IAM
            modelBuilder.ApplyConfiguration(new RoleClaimsConfiguration());
            modelBuilder.ApplyConfiguration(new RolesConfiguration());
            modelBuilder.ApplyConfiguration(new UserClaimsConfiguration());
            modelBuilder.ApplyConfiguration(new UserLoginsConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new UserTokensConfiguration());
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
