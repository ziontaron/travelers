using System;
using System.Linq.Expressions;

namespace Reusable
{
    public interface IRepository<T> : IReadOnlyRepository<T> where T : BaseEntity
    {
        void Add(params T[] items);
        void Update(T item);
        void Delete(int id);
        void Delete(T entity);

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
