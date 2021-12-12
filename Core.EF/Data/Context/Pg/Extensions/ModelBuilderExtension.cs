using Core.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace my_multi_tenancy.Data.Configuration.Pg.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void ConvertToSnakeCase(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var currentTableName = modelBuilder.Entity(entity.Name).Metadata.GetTableName();
                var schema = modelBuilder.Entity(entity.Name).Metadata.GetSchema();
                modelBuilder.Entity(entity.Name).ToTable(currentTableName.ToSnakeCase(), schema.ToSnakeCase());

                foreach (var property in entity.GetProperties())
                    property.SetColumnName(property.Name?.ToSnakeCase());//TODO:Check performance

                foreach (var key in entity.GetKeys())
                    key.SetName(key.GetName().ToSnakeCase());

                foreach (var key in entity.GetForeignKeys())
                    key.SetConstraintName(key.GetConstraintName()?.ToSnakeCase());

                foreach (var index in entity.GetIndexes())
                    index.SetDatabaseName(index.GetDatabaseName().ToSnakeCase());
            }
        }
    }
}
