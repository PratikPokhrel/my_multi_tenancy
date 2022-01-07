using Core.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;

namespace Core.EF.Data.Extensions
{
    public static class ChangeTrackerExtensions
    {
        public static void ProcessCreation(this ChangeTracker changeTracker, Guid userId,Guid? tenantId)
        {
            foreach (var item in changeTracker.Entries<IHasCreatedOn>().Where(e => e.State == EntityState.Added))
            {
                item.Entity.CreatedOn = DateTime.UtcNow;
            }

            foreach (var item in changeTracker.Entries<IHasCreator>().Where(e => e.State == EntityState.Added))
            {
                item.Entity.CreatedBy = userId;
            }

            foreach (var item in changeTracker.Entries<IHasTenant>().Where(e => e.State == EntityState.Added))
            {
                if (tenantId.HasValue)
                {
                    item.Entity.TenantId = tenantId.Value;
                }
            }
        }

        public static void ProcessModification(this ChangeTracker changeTracker, Guid userId)
        {
            foreach (var item in changeTracker.Entries<IHasModifiedOn>().Where(e => e.State == EntityState.Modified))
            {
                item.Entity.ModifiedOn = DateTime.UtcNow;
            }

            foreach (var item in changeTracker.Entries<IHasModifier>().Where(e => e.State == EntityState.Modified))
            {
                item.Entity.ModifiedBy = userId;
            }
        }

        public static void ProcessDeletion(this ChangeTracker changeTracker, Guid userId)
        {
            foreach (var item in changeTracker.Entries<IHasDeletedOn>().Where(e => e.State == EntityState.Deleted))
            {
                item.Entity.DeletedOn = DateTime.UtcNow;
            }

            foreach (var item in changeTracker.Entries<IHasDeleter>().Where(e => e.State == EntityState.Deleted))
            {
                item.Entity.DeletedBy = userId;
            }

            foreach (var item in changeTracker.Entries<ISoftDeleted>().Where(e => e.State == EntityState.Deleted))
            {
                item.State = EntityState.Modified;
                item.Entity.IsDeleted = true;
            }
        }
    }
}
