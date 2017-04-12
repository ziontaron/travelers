using Reusable;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessSpecificLogic.FS
{
    public class FSReadOnlyRepository<T> : IReadOnlyRepository<T> where T : FSEntity, new()
    {
        protected readonly FSContext context;
        public int? byUserId { get; set; }

        public string EntityName { get; set; }

        public FSReadOnlyRepository(FSContext context)
        {
            this.context = context;
            EntityName = typeof(T).Name;
        }

        public IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            var dataset = context.Database.SqlQuery<T>(new T().sqlGetAll);
            return dataset.ToList();
        }

        public IEnumerable<T> GetList(Expression<Func<T, object>> orderBy, params Expression<Func<T, bool>>[] wheres)
        {
            throw new NotImplementedException();
        }

        public T GetByID(int id)
        {
            var dataset = context.Database.SqlQuery<T>(new T().sqlGetById, 
                new SqlParameter("@id", id));
            return dataset.FirstOrDefault();
        }

        public T GetSingle(params Expression<Func<T, bool>>[] wheres)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetListByParent<P>(int parentID) where P : class
        {
            throw new NotImplementedException();
        }

        public T GetSingleByParent<P>(int parentID) where P : class
        {
            throw new NotImplementedException();
        }
    }
}
