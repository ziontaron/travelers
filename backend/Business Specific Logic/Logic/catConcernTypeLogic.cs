using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;

namespace BusinessSpecificLogic.Logic
{
    public interface ICatConcernTypeLogic : IBaseLogic<cat_ConcernType> { }

    public class catConcernTypeLogic : BaseLogic<cat_ConcernType>, ICatConcernTypeLogic
    {
        public catConcernTypeLogic(DbContext context, IRepository<cat_ConcernType> repository) : base(context, repository)
        {
        }

        protected override void loadNavigationProperties(DbContext context, params cat_ConcernType[] entities)
        {
        }
    }

}
