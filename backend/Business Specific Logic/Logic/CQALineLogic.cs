using BusinessSpecificLogic.EF;
using Reusable;
using System.Data.Entity;

namespace BusinessSpecificLogic.Logic
{
    public interface ICQALineLogic : IBaseLogic<CQALine> { }

    public class CQALineLogic : BaseLogic<CQALine>, ICQALineLogic
    {
        public CQALineLogic(DbContext context, IRepository<CQALine> repository) : base(context, repository) { }
    }

}