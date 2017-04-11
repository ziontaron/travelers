using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Reusable
{
    public interface IReadOnlyRepository<T> where T : class
    {
        int? byUserId { get; set; }
        string EntityName { get; set; }

        IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);
        IEnumerable<T> GetList(Expression<Func<T, object>> orderBy, params Expression<Func<T, bool>>[] wheres);
        T GetByID(int id);
        T GetSingle(params Expression<Func<T, bool>>[] wheres);
        
        IList<T> GetListByParent<P>(int parentID) where P : class;
        T GetSingleByParent<P>(int parentID) where P : class;
    }
}
