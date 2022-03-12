using Core.EF.Configs.QueryModelConfigs;
using Core.EF.Data.Context;
using Core.EF.Data.Extensions;
using Core.Entities;
using Core.Entities.Audit;
using eixample.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using TM.Core.Entities;

namespace Core.EF.Data.Configuration.Pg
{
    public class DeviceContext : DbContext, IDbContext
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="dataSeeder">Initial data seeder</param>
        public DeviceContext(DbContextOptions<DeviceContext> options)
            : base(options)
        {
            // TODO: Comment below this if you are running migrations commands
            // TODO: uncomment below line of you are running the application for the first time
            //this.Database.EnsureCreated();
        }

        /// <summary>
        /// Get or sets the devices data model
        /// </summary>
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Facility> Facility { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<ResponseLogs> ResponseLogs { get; set; }
        public DbSet<Issue> Issue { get; set; }
        public DbSet<IssueUser> IssueUser { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectUser> ProjectUser { get; set; }

        /// <summary>
        /// Get or sets the device groups data model
        /// </summary>
        /// <summary>
        /// Relation between tables.
        /// </summary>
        /// <param name="modelBuilder">Entity framework model builder before creating database</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConvertToSnakeCase();
            modelBuilder.EnableSoftDelete();
            modelBuilder.Entity<IssueUser>().HasKey(e => new { e.UserId, e.IssueId });
            modelBuilder.Entity<ProjectUser>().HasKey(e => new { e.UserId, e.ProjecId });
            modelBuilder.ApplyConfiguration(new CategoryItemQueryConfiguration());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.LogTo(message => Debug.WriteLine(message));
        public DbContextType GetContextType => DbContextType.Account;

    }


}
