using System.ComponentModel;

namespace Core.EF.Data
{
    /// <summary>
    /// Gets or sets the database type (No sql or Sql express)
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// MsSql Database type
        /// </summary>
        MsSql = 0,

        /// <summary>
        /// Postgres Database type
        /// </summary>
        Postgres = 1,

        /// <summary>
        /// Mongodb database type
        /// </summary>
        //MongoDb = 2
    }
}
