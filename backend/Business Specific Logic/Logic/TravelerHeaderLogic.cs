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
    public interface ITravelerHeaderLogic : IBaseLogic<TravelerHeader>
    {

    }

    public class TravelerHeaderLogic : BaseLogic<TravelerHeader>, ITravelerHeaderLogic
    {
        public TravelerHeaderLogic(DbContext context, IRepository<TravelerHeader> repository) : base(context, repository)
        {
        }
    }
}
