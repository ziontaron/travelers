using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Reusable
{
    public interface IRepository<T> where T : BaseEntity
    {
        int? byUserId { get; set; }
        string EntityName { get; set; }

        IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);
        IEnumerable<T> GetList(Expression<Func<T, object>> orderBy, params Expression<Func<T, bool>>[] wheres);
        T GetByID(int id);
        T GetSingle(params Expression<Func<T, bool>>[] wheres);
        void Add(params T[] items);
        void Update(T item);
        void Delete(int id);
        void Delete(T entity);

        IList<T> GetListByParent<P>(int parentID) where P : class;
        T GetSingleByParent<P>(int parentID) where P : class;
        P AddToParent<P>(int parentId, T entity) where P : class;
        void RemoveFromParent<P>(int parentId, T entity) where P : class;
        void RemoveAllWhere(params Expression<Func<T, bool>>[] wheres);
        T SetPropertyValue(int entityId, string sProperty, string newValue);

        void Activate(int id);
        void Deactivate(int id);
        void Deactivate(T entity);
        void Lock(int id);
        void Lock(T entity);
        void Unlock(int id);
        void Unlock(T entity);

        void Finalize(T entity);
        void Unfinalize(T entity);
    }
}
