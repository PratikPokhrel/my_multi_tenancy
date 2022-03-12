using Core.Entities.Audit;
using Core.Infrastructure;
using eixample.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Core.EF.Data.Extensions
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

        public static void EnableSoftDelete(this ModelBuilder modelBuilder)
        {
            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeleted).IsAssignableFrom(type.ClrType) && (type.BaseType == null || !typeof(ISoftDeleted).IsAssignableFrom(type.BaseType.ClrType)))
                {
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);
                }
            }
        }

        public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> expression)
        {
            var entities = modelBuilder.Model
                .GetEntityTypes()
                .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
                .Select(e => e.ClrType);
            foreach (var entity in entities)
            {
                var newParam = Expression.Parameter(entity);
                var newbody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
                modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(newbody, newParam));
            }
        }
    }
}
