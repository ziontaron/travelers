using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Reusable
{
    public interface IRepository<T> where T : BaseEntity
    {
        int? byUserId { get; set; }
        string EntityName { get; set; }

        IList<T> GetAll();
        IEnumerable<T> GetList(Expression<Func<T, object>> orderBy, params Expression<Func<T, bool>>[] wheres);
        T GetByID(int id);
        T GetSingle(Func<T, bool> where);
        void Add(params T[] items);
        void Update(T item);
        void Delete(int id);

        IList<T> GetListByParent<P>(int parentID) where P : class;
        T GetSingleByParent<P>(int parentID) where P : class;
        P AddToParent<P>(int parentId, T entity) where P : class;
        void RemoveFromParent<P>(int parentId, T entity) where P : class;

        void Activate(int id);
        void Deactivate(int id);
    }
}
