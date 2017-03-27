using System;
using System.Linq.Expressions;

namespace Reusable
{
    public interface IBaseLogic<Entity> where Entity : BaseEntity
    {
        LoggedUser loggedUser { get; set; }
        CommonResponse Add(Entity entity);
        CommonResponse GetAll();
        CommonResponse GetByID(int ID);
        CommonResponse GetPage(int perPage, int page, string filterGeneral, Expression<Func<Entity, object>> orderby, params Expression<Func<Entity, bool>>[] wheres);
        CommonResponse GetSingleWhere(params Expression<Func<Entity, bool>>[] wheres);
        CommonResponse GetListWhere(Expression<Func<Entity, object>> orderby, params Expression<Func<Entity, bool>>[] wheres);
        CommonResponse Remove(Entity id);
        CommonResponse Remove(int id);
        CommonResponse Activate(int id);
        CommonResponse Update(Entity entity);
        CommonResponse AddToParent<ParentType>(int parentID, Entity entity) where ParentType : BaseEntity;
        CommonResponse GetAllByParent<ParentType>(int parentID) where ParentType : BaseEntity;
        CommonResponse RemoveFromParent<Parent>(int parentID, Entity entity) where Parent : BaseEntity;
        CommonResponse CreateInstance();
        CommonResponse GetAvailableFor<ForEntity>(int id) where ForEntity : BaseEntity;
        CommonResponse GetCatalogs();
        CommonResponse Unlock(Entity entity);
        CommonResponse Unlock(int id);
        CommonResponse Lock(Entity entity);
        CommonResponse Lock(int id);
        CommonResponse Finalize(Entity entity);
        CommonResponse Unfinalize(Entity entity);
        CommonResponse SetPropertyValue(Entity entity, string sProperty, string value);
        void FillRecursively<Parent>(IRecursiveEntity entity) where Parent : BaseEntity;
    }
}
