using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace Reusable
{
    public abstract class BaseLogic<Entity> : ReadOnlyBaseLogic<Entity>, IBaseLogic<Entity> where Entity : BaseEntity, new()
    {
        protected new IRepository<Entity> repository;

        public BaseLogic(DbContext context, IRepository<Entity> repository) : base(context, repository)
        {
            this.repository = repository;
        }

        protected enum OPERATION_MODE
        {
            NONE,
            ADD,
            UPDATE
        };

        protected virtual void onAfterSaving(DbContext context, Entity entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE) { }
        protected virtual void onBeforeSaving(Entity entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE) { }
        protected virtual void onBeforeRemoving(Entity entity, BaseEntity parent = null) { }
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

                        onBeforeSaving(entity, null, OPERATION_MODE.ADD);

                        repository.Add(entity);
                        onAfterSaving(context, entity, null, OPERATION_MODE.ADD);

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

                        onBeforeSaving(entity, null, OPERATION_MODE.UPDATE);

                        repository.Update(entity);
                        onAfterSaving(context, entity, null, OPERATION_MODE.UPDATE);

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
                        onAfterSaving(context, entity, parent, OPERATION_MODE.ADD);

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

                        onAfterSaving(context, entity, null, OPERATION_MODE.UPDATE);

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
