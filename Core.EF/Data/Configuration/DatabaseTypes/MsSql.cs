using System.Data.Common;
using System.Data.SqlClient;
using Core.EF.Data;
using Core.EF.Data.Configuration;
using Core.EF.Data.Configuration.DatabaseTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DeviceManager.Api.Configuration.DatabaseTypes
{
    /// <inherit/>
    public class MsSql : IDatabaseType
    {
        /// <inherit/>
        public IServiceCollection EnableDatabase(IServiceCollection services, ConnectionSettings connectionOptions)
        {
            return services;
        }

        /// <inherit/>
        public DbConnectionStringBuilder GetConnectionBuilder(string connectionString)
        {
            return new SqlConnectionStringBuilder(connectionString);
        }

        /// <inherit/>
        public DbContextOptionsBuilder GetContextBuilder(DbContextOptionsBuilder optionsBuilder, ConnectionSettings connectionOptions, string connectionString)
        {
            return optionsBuilder.UseSqlServer(connectionString);
        }

        /// <inherit/>
        public DbContextOptionsBuilder<TContext> SetConnectionString<TContext>(DbContextOptionsBuilder<TContext> contextOptionsBuilder, string connectionString) where TContext : DbContext
        {
            return contextOptionsBuilder.UseSqlServer(connectionString);
        }
    }
}