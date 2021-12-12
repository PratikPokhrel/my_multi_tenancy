using System;
using System.Linq;
using Core.Constants;
using Core.EF.Data.Configuration.DatabaseTypes;
using Core.EF.Data.Configuration.Pg;
using Core.EF.Data.Context;
using Core.EF.Data.Context.Default;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
                        databaseTypeInstance.GetContextBuilder(options, connectionOptions, connectionString));
                services.AddScoped<IDbContext, DeviceContext>();


                services.AddDbContext<DefaultContext>(options =>
                     databaseTypeInstance.GetContextBuilder(options, connectionOptions, identityConnection));
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


            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DeviceContext>();
            dbContext.Database.SetConnectionString(connectionOptions.DefaultConnection);
            if (dbContext.Database.GetMigrations().Any())
            {
                dbContext.Database.Migrate();
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