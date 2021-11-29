using Microsoft.EntityFrameworkCore;
using System;

namespace my_multi_tenancy.Data.Context
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
        public DbSet<Movie> movie { get; set; }

        /// <summary>
        /// Get or sets the device groups data model
        /// </summary>
        /// <summary>
        /// Relation between tables.
        /// </summary>
        /// <param name="modelBuilder">Entity framework model builder before creating database</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().ToTable("movie");
            modelBuilder.Entity<Movie>().Property(e=>e.Id).HasColumnName("id");
            modelBuilder.Entity<Movie>().Property(e=>e.Name).HasColumnName("name");
        }
    }

    public class AccountContext : DbContext, IDbContext
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="dataSeeder">Initial data seeder</param>
        public AccountContext(DbContextOptions<AccountContext> options)
            : base(options)
        {
            // TODO: Comment below this if you are running migrations commands
            // TODO: uncomment below line of you are running the application for the first time
            //this.Database.EnsureCreated();
        }

        /// <summary>
        /// Get or sets the devices data model
        /// </summary>
        public DbSet<Account> Account { get; set; }

        /// <summary>
        /// Get or sets the device groups data model
        /// </summary>
        /// <summary>
        /// Relation between tables.
        /// </summary>
        /// <param name="modelBuilder">Entity framework model builder before creating database</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }

    public class Account
    {
        public Guid AccountId { get; set; }
    }
        public class Movie
    {
        public Guid  Id { get; set; }
        public string Name { get; set; }
    }
}
