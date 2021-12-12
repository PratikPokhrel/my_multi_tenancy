using System;

namespace Core.Entities.Audit
{
    public interface IHasModifier
    {
        Guid? ModifiedBy {get;set;}
    }
}