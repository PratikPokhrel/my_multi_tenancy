using Core.Constants;
using Core.EF.Data.Configuration.DatabaseTypes;
using Core.Entities;
using DeviceManager.Api.Configuration.DatabaseTypes;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EF.Data.Extensions
{
    internal static class DbConnectionStringBuilderEntension
    {
        public static DbConnectionStringBuilder BuildConnectionString(this Tenant tenant)
        {
            if (tenant.DatabaseType == (int)DatabaseType.MsSql)
            {
                IDatabaseType dbType = new MsSql();
                DbConnectionStringBuilder connectionBuilder = dbType.GetConnectionBuilder(DefaultConstants.MsSqlConnectionStringFormat);
                connectionBuilder.Clear();
                connectionBuilder.Add(DefaultConstants.Database, tenant.Database);
                connectionBuilder.Add(DefaultConstants.DbServer, tenant.Server);
                connectionBuilder.Add(DefaultConstants.DbUser, tenant.User);
                connectionBuilder.Add(DefaultConstants.Password, tenant.Password.Trim());

                return connectionBuilder;
            }
            else if (tenant.DatabaseType == (int)DatabaseType.Postgres)
            {
                IDatabaseType dbType = new Postgres();
                DbConnectionStringBuilder connectionBuilder = dbType.GetConnectionBuilder(DefaultConstants.PSqlConnectionStringFormat);
                connectionBuilder.Clear();
                connectionBuilder.Add(DefaultConstants.Database, tenant.Database);
                connectionBuilder.Add(DefaultConstants.DbHost, tenant.Server);
                connectionBuilder.Add(DefaultConstants.DbUserID, tenant.User);
                connectionBuilder.Add(DefaultConstants.Password, tenant.Password);
                connectionBuilder.Add(DefaultConstants.DbPort, tenant.Port);
                return connectionBuilder;
            }
            else
                throw new Exception("Invalid database type");
        }
    }
}
