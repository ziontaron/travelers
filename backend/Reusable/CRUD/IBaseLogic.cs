using System;
using System.Linq.Expressions;

namespace Reusable
{
    public interface IBaseLogic<Entity> : IReadOnlyBaseLogic<Entity> where Entity : BaseEntity
    {
        CommonResponse Add(Entity entity);
        CommonResponse Remove(Entity id);
        CommonResponse Remove(int id);
        CommonResponse Activate(int id);
        CommonResponse Update(Entity entity);
        CommonResponse AddToParent<ParentType>(int parentID, Entity entity) where ParentType : BaseEntity;
        CommonResponse RemoveFromParent<Parent>(int parentID, Entity entity) where Parent : BaseEntity;
        CommonResponse Unlock(Entity entity);
        CommonResponse Unlock(int id);
        CommonResponse Lock(Entity entity);
        CommonResponse Lock(int id);
        CommonResponse Finalize(Entity entity);
        CommonResponse Unfinalize(Entity entity);
        CommonResponse SetPropertyValue(Entity entity, string sProperty, string value);
        CommonResponse GetAvailableFor<ForEntity>(int id) where ForEntity : BaseEntity;
    }
}
