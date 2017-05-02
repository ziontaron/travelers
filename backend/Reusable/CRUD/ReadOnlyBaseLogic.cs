using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using static Reusable.BaseEntity;

namespace Reusable
{
    public abstract class ReadOnlyBaseLogic<Entity> : IReadOnlyBaseLogic<Entity> where Entity : class, new()
    {
        public LoggedUser loggedUser { get; set; }

        protected DbContext context;
        protected IReadOnlyRepository<Entity> repository;

        public ReadOnlyBaseLogic(DbContext context, IReadOnlyRepository<Entity> repository)//, int? byUserId)
        {
            this.context = context;
            this.repository = repository;
            //this.byUserId = loggedUser.UserID;
        }

        protected virtual void loadNavigationProperties(params Entity[] entities) { }

        protected static EntityState GetEntityState(EF_EntityState state)
        {
            switch (state)
            {
                case EF_EntityState.Unchanged:
                    return EntityState.Unchanged;
                case EF_EntityState.Added:
                    return EntityState.Added;
                case EF_EntityState.Modified:
                    return EntityState.Modified;
                case EF_EntityState.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Detached;
            }
        }

        protected virtual void onCreate(Entity entity) { }

        public virtual List<Expression<Func<Entity, object>>> NavigationPropertiesWhenGetAll { get { return new List<Expression<Func<Entity, object>>>(); } }

        public virtual CommonResponse GetAll()
        {
            CommonResponse response = new CommonResponse();
            IList<Entity> entities;
            try
            {
                //var repository = RepositoryFactory.Create<Entity>(context, byUserId);

                repository.byUserId = loggedUser.UserID;
                entities = repository.GetAll(NavigationPropertiesWhenGetAll.ToArray());
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entities);
        }

        public virtual CommonResponse GetByID(int ID)
        {
            CommonResponse response = new CommonResponse();
            List<Entity> entities = new List<Entity>();
            try
            {

                Entity entity = repository.GetByID(ID);
                if (entity != null)
                {
                    repository.byUserId = loggedUser.UserID;
                    entities.Add(entity);
                    loadNavigationProperties(entities.ToArray());
                    return response.Success(entity);
                }
                else
                {
                    return response.Error("Entity not found.");
                }

            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

        protected class FilterResponse
        {
            public List<List<BaseEntity>> Dropdowns { get; set; }
            public int total_items { get; set; }
            public int total_filtered_items { get; set; }
        }

        public virtual CommonResponse GetPage(int perPage, int page, string filterGeneral, Expression<Func<Entity, bool>>[] wheres, Expression<Func<Entity, object>> orderby, params Expression<Func<Entity, bool>>[] database_wheres)
        {
            CommonResponse response = new CommonResponse();
            FilterResponse filterResponse = new FilterResponse();

            IEnumerable<Entity> entities; //Entities comming from DB
            IQueryable<Entity> resultset; //To filter properties not in DB
            try
            {
                repository.byUserId = loggedUser.UserID;

                #region Apply Database Filtering

                entities = repository.GetList(orderby, database_wheres);
                loadNavigationProperties(entities.ToArray());
                #endregion

                #region Apply Roles Filtering

                #endregion

                #region Applying Non-Database Properties Filtering
                resultset = entities.AsQueryable();
                foreach (var where in wheres)
                {
                    resultset = resultset.Where(where);
                }

                #endregion

                filterResponse.total_items = resultset.Count();

                #region Apply General Search Filter

                if (!string.IsNullOrWhiteSpace(filterGeneral))
                {
                    string[] arrFilterGeneral = filterGeneral.ToLower().Split(' ');

                    var searchableProps = typeof(Entity).GetProperties().Where(prop => new[] { "String" }.Contains(prop.PropertyType.Name));

                    resultset = resultset.Where(e => searchableProps.Any(prop =>
                                                        arrFilterGeneral.All(keyword =>
                                                            ((string)prop.GetValue(e, null) ?? "").ToString().ToLower()
                                                            .Contains(keyword))));
                }

                #endregion

                filterResponse.total_filtered_items = resultset.Count();

                #region Pagination

                var result = resultset.Skip((page - 1) * perPage).Take(perPage).ToList();
                #endregion

                return response.Success(result, filterResponse);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

        public virtual CommonResponse GetSingleWhere(params Expression<Func<Entity, bool>>[] wheres)
        {
            CommonResponse response = new CommonResponse();
            FilterResponse filterResponse = new FilterResponse();

            Entity entity;
            try
            {
                repository.byUserId = loggedUser.UserID;

                entity = repository.GetSingle(wheres);
                if (entity != null)
                {
                    loadNavigationProperties(entity);
                }

                return response.Success(entity);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

        public virtual CommonResponse GetListWhere(Expression<Func<Entity, object>> orderby, params Expression<Func<Entity, bool>>[] wheres)
        {
            CommonResponse response = new CommonResponse();
            IEnumerable<Entity> entities;
            try
            {
                repository.byUserId = loggedUser.UserID;
                entities = repository.GetList(orderby, wheres);
                loadNavigationProperties(entities.ToArray());
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entities);
        }

        public virtual CommonResponse GetAllByParent<ParentType>(int parentID) where ParentType : BaseEntity
        {
            CommonResponse response = new CommonResponse();
            IList<Entity> entities;

            try
            {

                //var repository = RepositoryFactory.Create<Entity>(context, byUserId);

                repository.byUserId = loggedUser.UserID;
                entities = repository.GetListByParent<ParentType>(parentID);
                loadNavigationProperties(entities.ToArray());
                //MethodInfo method = repository.GetType().GetMethod("GetListByParent");
                //MethodInfo genericMethod = method.MakeGenericMethod(new Type[] { typeof(ParentType) });
                //entities = (IList<Entity>) genericMethod.Invoke(repository, new object[] { parentID });

            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entities);
        }

        public virtual CommonResponse GetSingleByParent<ParentType>(int parentID) where ParentType : BaseEntity
        {
            CommonResponse response = new CommonResponse();
            Entity entity;

            try
            {

                repository.byUserId = loggedUser.UserID;
                entity = repository.GetSingleByParent<ParentType>(parentID);
                loadNavigationProperties(entity);
                //MethodInfo method = repository.GetType().GetMethod("GetListByParent");
                //MethodInfo genericMethod = method.MakeGenericMethod(new Type[] { typeof(ParentType) });
                //entities = (IList<Entity>) genericMethod.Invoke(repository, new object[] { parentID });

            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entity);
        }

        public CommonResponse CreateInstance()
        {
            CommonResponse response = new CommonResponse();
            Entity entity = new Entity();
            onCreate(entity);
            return response.Success(entity);
        }

        protected virtual ICatalogContainer LoadCatalogs() { return null; }

        public virtual CommonResponse GetCatalogs()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response.Success(LoadCatalogs());
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success();
        }

        public void FillRecursively<Parent>(IRecursiveEntity entity) where Parent : BaseEntity
        {
            repository.byUserId = loggedUser.UserID;
            if (entity != null)
            {
                IList<Entity> entities = repository.GetListByParent<Parent>(entity.id);
                //loadNavigationProperties(context, entities.ToArray());
                entity.nodes = new List<IRecursiveEntity>();
                foreach (IRecursiveEntity item in entities)
                {
                    entity.nodes.Add(item);
                    FillRecursively<Parent>(item);
                }
            }
        }

        public List<Entity> NestedToSingleList(IRecursiveEntity entity, List<Entity> result)
        {
            repository.byUserId = loggedUser.UserID;
            if (result == null) { result = new List<Entity>(); }
            if (entity != null)
            {
                foreach (var item in entity.nodes)
                {
                    result.Add((Entity)item);
                    NestedToSingleList(item, result);
                }
            }
            return result;
        }

    }
}
