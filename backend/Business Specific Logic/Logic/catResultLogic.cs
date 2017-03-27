using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;

namespace BusinessSpecificLogic.Logic
{
    public interface ICatResultLogic : IBaseLogic<cat_Result> { }

    public class catResultLogic : BaseLogic<cat_Result>, ICatResultLogic
    {
        public catResultLogic(DbContext context, IRepository<cat_Result> repository) : base(context, repository)
        {
        }
    }

}
