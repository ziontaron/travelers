using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;

namespace BusinessSpecificLogic.Logic
{
    public interface ICatStatusLogic : IBaseLogic<cat_Status> { }

    public class catStatusLogic : BaseLogic<cat_Status>, ICatStatusLogic
    {
        public catStatusLogic(DbContext context, IRepository<cat_Status> repository) : base(context, repository)
        {
        }
    }

}
