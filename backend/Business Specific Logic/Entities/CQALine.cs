using Reusable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessSpecificLogic.EF
{
    public partial class CQALine : BaseDocument
    {
        public CQALine()
        {
            sys_active = true;
        }

        public override int id
        {
            get
            {
                return CQALinelKey;
            }
        }
    }
}
