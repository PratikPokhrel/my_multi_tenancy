using System;

namespace my_multi_tenancy.Data.Context
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }

        /// <summary>
        /// DataBase Type
        /// 1=Mssql,2=Postgres
        /// </summary>
        public int DatabaseType { get; set; }
    }
}
