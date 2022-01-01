using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Core.Constants;
using Core.EF.Data.Configuration.DatabaseTypes;
using Core.EF.Data.Configuration.Pg;
using Core.EF.Data.Context;
using Core.EF.Data.Context.Default;
using Core.EF.Data.Extensions;
using Core.EF.Seed;
using Core.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.EF.Data.Configuration
{
    /// <summary>
    /// Configurations related to entity framework
    /// </summary>
    public static class EntityFrameworkConfiguration
    {
        /// <summary>
        /// Configures the service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void ConfigureService(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString(DefaultConstants.DefaultConnection);
            string identityConnection = configuration.GetConnectionString(DefaultConstants.IdentityConnection);

            // Database connection settings
            var connectionOptions = services.BuildServiceProvider().GetRequiredService<ConnectionSettings>();
           

            RegisterDatabaseType(services, connectionOptions);

            var databaseTypeInstance = services.BuildServiceProvider().GetRequiredService<IDatabaseType>();

            databaseTypeInstance.EnableDatabase(services, connectionOptions);

            if (connectionOptions.AllowMultipleConnection)
            {
                services.AddDbContext<DeviceContext>(options =>
                {
                    databaseTypeInstance.GetContextBuilder(options, connectionOptions, connectionString);
                    options.EnableSensitiveDataLogging();
                });
                services.AddScoped<IDbContext, DeviceContext>();


                services.AddDbContext<DefaultContext>(options => {
                    databaseTypeInstance.GetContextBuilder(options, connectionOptions, identityConnection);
                    options.EnableSensitiveDataLogging();
                });
                services.AddScoped<IDbContext, DefaultContext>();


                //services.AddDbContext<AccountContext>(options =>
                //databaseTypeInstance.GetContextBuilder(options, connectionOptions, connectionString));
                //services.AddScoped<IDbContext, AccountContext>();


            }
            else
            {
                // Entity framework configuration
                services.AddDbContext<DeviceContext>(options =>
                    databaseTypeInstance.GetContextBuilder(options, connectionOptions, connectionString));
                services.AddScoped<IDbContext, DeviceContext>();
            }

            services.ApplyMigrationsAsync().GetAwaiter().GetResult();
        }



        public static async Task ApplyMigrationsAsync(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            DeviceContext dbContext = scope.ServiceProvider.GetRequiredService<DeviceContext>();
            Tenant[] tenants = new FileTenantSource().ListTenants();
            try
            {
                foreach (Tenant tenant in tenants)
                {
                    DbConnectionStringBuilder connectionBuilder = tenant.BuildConnectionString();
                    var contextOptionsBuilder = new DbContextOptionsBuilder<DeviceContext>();
                    dbContext.Database.SetConnectionString(connectionBuilder.ConnectionString);

                    if (dbContext.Database.GetMigrations().Any())
                        await dbContext.Database.MigrateAsync().ConfigureAwait(false);
                }

                //foreach (Tenant tenant in tenants)
                //{
                //    DbConnectionStringBuilder connectionBuilder = tenant.BuildConnectionString();
                //    dbContext.Database.SetConnectionString(connectionBuilder.ConnectionString);
                //    await dbContext.CreateSeederAsync().ConfigureAwait(false);
                //}
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.InnerException.Message);
            }
        }

       

        /// <summary>
        ///  Configures the assembly where migrations are maintained for this context.
        ///  <see href="https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/migrations/index" /> for migrations
        ///  <see href="https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet"/> for command line tools
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <typeparam name="TExtension"></typeparam>
        /// <param name="builder"></param>
        /// <returns>Migrations configured builder</returns>
        public static TBuilder GetMigrationInformation<TBuilder, TExtension>(RelationalDbContextOptionsBuilder<TBuilder, TExtension> builder)
             where TBuilder : RelationalDbContextOptionsBuilder<TBuilder, TExtension>
            where TExtension : RelationalOptionsExtension, new()
        {

            return builder.MigrationsAssembly(typeof(DeviceContext).Assembly.GetName().Name);
        }

        /// <summary>
        /// Inject database settings instance based on the connection string
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionOptions"></param>
        private static void RegisterDatabaseType(IServiceCollection services, ConnectionSettings connectionOptions)
        {
            var databaseInterfaceType = typeof(IDatabaseType);
            var instanceType = connectionOptions.DatabaseType.ToString();
            var instance = databaseInterfaceType.Assembly.GetTypes()
                                                .FirstOrDefault(x => databaseInterfaceType.IsAssignableFrom(x)
                                                                   && string.Equals(instanceType, x.Name, StringComparison.OrdinalIgnoreCase));
            services.AddSingleton((IDatabaseType)Activator.CreateInstance(instance));
        }

    }
}