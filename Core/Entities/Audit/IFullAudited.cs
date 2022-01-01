using System;

namespace Core.Entities.Audit
{
    public interface IFullAudited<T>:IEntity<T>,IHasCreator,IHasCreatedOn,IHasModifier,IHasModifiedOn,IHasDeleter,IHasDeletedOn, ISoftDeleted
    {

    }
}