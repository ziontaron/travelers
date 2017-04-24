using System;
using System.Linq.Expressions;

namespace Reusable
{
    public interface IReadOnlyBaseLogic<Entity> where Entity : class
    {
        LoggedUser loggedUser { get; set; }
        CommonResponse GetAll();
        CommonResponse GetByID(int ID);
        CommonResponse GetPage(int perPage, int page, string filterGeneral, Expression<Func<Entity, bool>>[] wheres, Expression<Func<Entity, object>> orderby, params Expression<Func<Entity, bool>>[] database_wheres);
        CommonResponse GetSingleWhere(params Expression<Func<Entity, bool>>[] wheres);
        CommonResponse GetListWhere(Expression<Func<Entity, object>> orderby, params Expression<Func<Entity, bool>>[] wheres);
        CommonResponse GetAllByParent<ParentType>(int parentID) where ParentType : BaseEntity;
        CommonResponse CreateInstance();
        CommonResponse GetCatalogs();
        void FillRecursively<Parent>(IRecursiveEntity entity) where Parent : BaseEntity;
    }
}
