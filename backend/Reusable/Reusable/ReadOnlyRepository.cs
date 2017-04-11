using LinqKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Reusable
{
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
    {
        protected readonly DbContext context;
        public int? byUserId { get; set; }

        public string EntityName { get; set; }

        public ReadOnlyRepository(DbContext context)
        {
            this.context = context;
            EntityName = typeof(T).Name;
        }

        public virtual IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            IQueryable<T> dbQuery = context.Set<T>();

            //Eager Loading:
            foreach (var navigationProperty in navigationProperties)
            {
                dbQuery.Include(navigationProperty);
            }

            list = dbQuery
                .AsNoTracking()
                .ToList();

            /*DOCUMENT*/
            if (typeof(T).IsSubclassOf(typeof(BaseDocument)))
            {
                list = list.Where(d => (d as BaseDocument).sys_active == true).ToList();

                foreach (T item in list)
                {
                    var document = item as BaseDocument;

                    //document.InfoTrack = context.Database.SqlQuery<Track>("select * from Track where Entity_ID = @p0 and Entity_Kind = @p1",
                    //    document.ID, document.AAA_EntityName).FirstOrDefault();
                    document.InfoTrack = context.Set<Track>()
                                        .AsNoTracking()
                                        .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                }
            }

            return list;
        }

        public virtual IEnumerable<T> GetList(Expression<Func<T, object>> orderBy, params Expression<Func<T, bool>>[] wheres)
        {
            var predicate = PredicateBuilder.New<T>(true);

            foreach (var where in wheres)
            {
                predicate = predicate.And(where);
            }

            IEnumerable<T> list;
            IQueryable<T> dbQuery = context.Set<T>();

            list = dbQuery.AsExpandable()
            .AsNoTracking()
            .Where(predicate);
            if (orderBy != null)
            {
                list = list.AsQueryable().OrderBy(orderBy);
            }

            /*DOCUMENT*/
            if (typeof(T).IsSubclassOf(typeof(BaseDocument)))
            {
                predicate = predicate.And(d => (d as BaseDocument).sys_active == true);

                list = list.Where(predicate).ToList();

                foreach (T item in list)
                {
                    var document = item as BaseDocument;

                    document.InfoTrack = context.Set<Track>()
                                        .AsNoTracking()
                                        .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                }
            }
            return list;

        }

        public virtual T GetSingle(params Expression<Func<T, bool>>[] wheres)
        {
            var predicate = PredicateBuilder.New<T>(true);

            foreach (var where in wheres)
            {
                predicate = predicate.And(where);
            }

            T item = null;
            IQueryable<T> dbQuery = context.Set<T>();

            item = dbQuery.AsExpandable()
                .AsNoTracking()
                .FirstOrDefault(predicate);

            if (item != null)
            {
                /*DOCUMENT*/
                if (typeof(T).IsSubclassOf(typeof(BaseDocument)))
                {
                    var document = item as BaseDocument;
                    if (document.sys_active == true)
                    {
                        document.InfoTrack = context.Set<Track>()
                                            .AsNoTracking()
                                            .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return item;
        }

        public virtual T GetByID(int id)
        {
            DbSet<T> set = context.Set<T>();
            T item = set.Find(id);

            /*DOCUMENT*/
            if (typeof(T).IsSubclassOf(typeof(BaseDocument)))
            {
                var document = item as BaseDocument;
                if (document != null && document.sys_active == true)
                {
                    document.InfoTrack = context.Set<Track>()
                                        .AsNoTracking()
                                        .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);
                }
                else
                {
                    return null;
                }
            }

            return item;
        }

        public virtual IList<T> GetListByParent<P>(int parentID) where P : class
        {
            List<T> list = new List<T>();

            DbSet<P> setParent = context.Set<P>();

            P parent = setParent.Find(parentID);

            if (parent == null)
            {
                throw new Exception("Parent non-existent.");
            }

            if (parent is BaseDocument)
            {
                if ((parent as BaseDocument).sys_active == false)
                {
                    throw new Exception("Parent non-existent.");
                }
            }

            string tName = typeof(T).Name + "s";
            list = context.Entry(parent).Collection<T>(tName)
                .Query()
                .AsNoTracking()
                .ToList<T>();

            /*DOCUMENT*/
            if (typeof(T).IsSubclassOf(typeof(BaseDocument)))
            {
                list = list.Where(d => (d as BaseDocument).sys_active == true).ToList();

                foreach (T item in list)
                {
                    var document = item as BaseDocument;

                    document.InfoTrack = context.Set<Track>()
                                        .AsNoTracking()
                                        .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);

                }
            }

            //Removing Recurivity
            string navigationPropertyName = typeof(P).Name;
            foreach (T item in list)
            {
                //PropertyInfo prop = item.GetType().GetProperty(navigationPropertyName, BindingFlags.Public | BindingFlags.Instance);
                //if (null != prop && prop.CanWrite)
                //{
                //    prop.SetValue(item, new List<P>());
                //}

                try
                {
                    //Trying for collection
                    context.Entry(item).Collection<P>(navigationPropertyName + "s").CurrentValue.Clear();
                }
                catch (Exception)
                {
                    //Trying for reference
                    //context.Entry(item).Reference<P>(navigationPropertyName).CurrentValue.
                }

            }

            return list;
        }

       public virtual T GetSingleByParent<P>(int parentID) where P : class
        {
            T entity = null;

            DbSet<P> setParent = context.Set<P>();

            P parent = setParent.Find(parentID);

            if (parent == null)
            {
                throw new Exception("Parent non-existent.");
            }

            if (parent is BaseDocument)
            {
                if ((parent as BaseDocument).sys_active == false)
                {
                    throw new Exception("Parent non-existent.");
                }
            }

            string tName = typeof(T).Name;
            entity = context.Entry(parent).Reference<T>(tName).Query().FirstOrDefault();

            /*DOCUMENT*/
            if (typeof(T).IsSubclassOf(typeof(BaseDocument)))
            {
                if (entity != null)
                {
                    var document = entity as BaseDocument;
                    document.InfoTrack = context.Set<Track>()
                                        .AsNoTracking()
                                        .FirstOrDefault(t => t.Entity_ID == document.id && t.Entity_Kind == document.AAA_EntityName);
                }
            }

            return entity;
        }
        
    }
}
