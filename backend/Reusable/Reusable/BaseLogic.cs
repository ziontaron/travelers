using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static Reusable.BaseEntity;

namespace Reusable
{
    public abstract class BaseLogic<Entity> : IBaseLogic<Entity> where Entity : BaseEntity, new()
    {
        public LoggedUser loggedUser { get; set; }

        protected DbContext context;
        protected IRepository<Entity> repository;

        public BaseLogic(DbContext context, IRepository<Entity> repository)//, int? byUserId)
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

        protected virtual void onAfterSaving(DbContext context, Entity entity, BaseEntity parent = null) { }
        protected virtual void onBeforeSaving(Entity entity, BaseEntity parent = null) { }
        protected virtual void onBeforeRemoving(Entity entity, BaseEntity parent = null) { }
        protected virtual void onCreate(Entity entity) { }
        protected virtual void onFinalize(Entity entity) { }
        protected virtual void onUnfinalize(Entity entity) { }

        public virtual CommonResponse Add(Entity entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //var repository = RepositoryFactory.Create<Entity>(context, byUserId);
                        repository.byUserId = loggedUser.UserID;

                        onBeforeSaving(entity);

                        repository.Add(entity);
                        onAfterSaving(context, entity);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (DbEntityValidationException ex)
                    {
                        // Retrieve the error messages as a list of strings.
                        var errorMessages = ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        // Join the list to a single string.
                        var fullErrorMessage = string.Join("; ", errorMessages);

                        transaction.Rollback();
                        return response.Error(fullErrorMessage);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return response.Error(ex.ToString());
                    }
                }
            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entity);
        }

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

        private class FilterResponse
        {
            public List<List<BaseEntity>> Dropdowns { get; set; }
            public int total_items { get; set; }
            public int total_filtered_items { get; set; }
        }

        public virtual CommonResponse GetPage(int perPage, int page, string filterGeneral, Expression<Func<Entity, object>> orderby, params Expression<Func<Entity, bool>>[] wheres)
        {
            CommonResponse response = new CommonResponse();
            FilterResponse filterResponse = new FilterResponse();

            IEnumerable<Entity> entities;
            try
            {
                repository.byUserId = loggedUser.UserID;

                #region Apply Database Filtering

                entities = repository.GetList(orderby, wheres);

                #endregion

                #region Apply Roles Filtering

                #endregion

                filterResponse.total_items = entities.Count();

                #region Apply General Search Filter

                if (!string.IsNullOrWhiteSpace(filterGeneral))
                {
                    string[] arrFilterGeneral = filterGeneral.ToLower().Split(' ');

                    var searchableProps = typeof(Entity).GetProperties().Where(prop => new[] { "String" }.Contains(prop.PropertyType.Name));

                    entities = entities.Where(e => searchableProps.Any(prop =>
                                                        arrFilterGeneral.All(keyword =>
                                                            ((string)prop.GetValue(e, null) ?? "").ToString().ToLower()
                                                            .Contains(keyword))));
                }

                #endregion

                filterResponse.total_filtered_items = entities.Count();

                #region Pagination

                var result = entities.Skip((page - 1) * perPage).Take(perPage).ToList();
                loadNavigationProperties(result.ToArray());
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

        public virtual CommonResponse Remove(Entity entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        repository.byUserId = loggedUser.UserID;
                        onBeforeRemoving(entity);
                        repository.Delete(entity);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success(entity, repository.EntityName + " removed successfully.");
        }

        public virtual CommonResponse Remove(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //var repository = RepositoryFactory.Create<Entity>(context, byUserId);
                        repository.byUserId = loggedUser.UserID;
                        repository.Delete(id);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success(id, repository.EntityName + " removed successfully.");
        }

        public virtual CommonResponse Activate(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        repository.byUserId = loggedUser.UserID;
                        repository.Activate(id);

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success(id);
        }

        public virtual CommonResponse Update(Entity entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        repository.byUserId = loggedUser.UserID;

                        onBeforeSaving(entity);

                        repository.Update(entity);
                        onAfterSaving(context, entity);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (DbEntityValidationException ex)
                    {
                        // Retrieve the error messages as a list of strings.
                        var errorMessages = ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        // Join the list to a single string.
                        var fullErrorMessage = string.Join("; ", errorMessages);

                        transaction.Rollback();
                        return response.Error(fullErrorMessage);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {

                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entity);
        }

        public virtual CommonResponse AddToParent<ParentType>(int parentID, Entity entity) where ParentType : BaseEntity
        {
            CommonResponse response = new CommonResponse();
            try
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //var repository = RepositoryFactory.Create<Entity>(context, byUserId);

                        //var parentRepoType = typeof(BaseEntityRepository<>);
                        //Type[] parentRepositoryArgs = { typeof(ParentType) };
                        //var makeme = parentRepoType.MakeGenericType(parentRepositoryArgs);
                        //object parentRepository = Activator.CreateInstance(makeme);

                        //PropertyInfo propContext = parentRepository.GetType().GetProperty("context", BindingFlags.Public | BindingFlags.Instance);
                        //propContext.SetValue(parentRepository, context);

                        //PropertyInfo propByUser = parentRepository.GetType().GetProperty("byUserID", BindingFlags.Public | BindingFlags.Instance);
                        //propByUser.SetValue(parentRepository, byUserID);

                        //MethodInfo method = parentRepository.GetType().GetMethod("GetByID");
                        //BaseEntity parent = (Entity)method.Invoke(parentRepository, new object[] { parentID });
                        //if (parent == null)
                        //{
                        //    return response.Error("Non-existent Parent Entity.");
                        //}

                        repository.byUserId = loggedUser.UserID;
                        ParentType parent = repository.AddToParent<ParentType>(parentID, entity);
                        onAfterSaving(context, entity, parent);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entity);
        }

        public virtual CommonResponse SetPropertyValue(Entity entity, string sProperty, string value)
        {
            CommonResponse response = new CommonResponse();
            try
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        repository.byUserId = loggedUser.UserID;

                        Entity result = repository.SetPropertyValue(entity.id, sProperty, value);

                        onAfterSaving(context, entity);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entity);
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

        public virtual CommonResponse GetAvailableFor<ForEntity>(int id) where ForEntity : BaseEntity
        {
            CommonResponse response = new CommonResponse();
            IEnumerable<Entity> availableEntities;
            try
            {
                repository.byUserId = loggedUser.UserID;

                IRepository<ForEntity> oRepository = new Repository<ForEntity>(context);
                oRepository.byUserId = loggedUser.UserID;


                ForEntity forEntity = oRepository.GetByID(id);
                if (forEntity == null)
                {
                    throw new Exception("Entity " + forEntity.AAA_EntityName + " not found.");
                }

                IList<Entity> childrenInForEntity = repository.GetListByParent<ForEntity>(id);

                IList<Entity> allEntities = repository.GetAll();

                availableEntities = allEntities.Where(e => !childrenInForEntity.Any(o => o.id == e.id));

                loadNavigationProperties(availableEntities.ToArray());
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(availableEntities);
        }

        public virtual CommonResponse RemoveFromParent<Parent>(int parentID, Entity entity) where Parent : BaseEntity
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        repository.byUserId = loggedUser.UserID;
                        repository.RemoveFromParent<Parent>(parentID, entity);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }
            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success();
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

        public virtual CommonResponse Lock(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        repository.byUserId = loggedUser.UserID;
                        repository.Lock(id);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success(id);
        }

        public virtual CommonResponse Lock(Entity entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        repository.byUserId = loggedUser.UserID;
                        repository.Lock(entity);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success(entity);
        }

        public virtual CommonResponse Unlock(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        repository.byUserId = loggedUser.UserID;
                        repository.Unlock(id);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success(id);
        }

        public virtual CommonResponse Unlock(Entity entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        repository.byUserId = loggedUser.UserID;
                        repository.Unlock(entity);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success(entity);
        }


        public virtual CommonResponse Finalize(Entity entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        repository.byUserId = loggedUser.UserID;

                        onFinalize(entity);

                        repository.Finalize(entity);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success(entity);
        }

        public virtual CommonResponse Unfinalize(Entity entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        repository.byUserId = loggedUser.UserID;

                        onUnfinalize(entity);

                        repository.Unfinalize(entity);

                        transaction.Commit();
                    }
                    catch (KnownError error)
                    {
                        transaction.Rollback();
                        return response.Error(error);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (KnownError error)
            {
                return response.Error(error);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success(entity);
        }

    }
}
