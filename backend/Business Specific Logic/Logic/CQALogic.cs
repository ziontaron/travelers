using BusinessSpecificLogic.EF;
using Reusable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BusinessSpecificLogic.Logic
{
    public interface ICQAHeaderLogic : IBaseLogic<CQAHeader>
    {

    }

    public class CQAHeaderLogic : BaseLogic<CQAHeader>, ICQAHeaderLogic
    {
        public CQAHeaderLogic(DbContext context, IRepository<CQAHeader> repository) : base(context, repository)
        {
        }

        protected override void loadNavigationProperties(DbContext context, params CQAHeader[] entities)
        {
            
        }
    }
}
