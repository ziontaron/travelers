using Reusable;

namespace BusinessSpecificLogic.EF
{
    public partial class cat_ConcernType : BaseEntity
    {
        public override int id
        {
            get
            {
                return ConcernTypeKey;
            }
        }
    }
}
