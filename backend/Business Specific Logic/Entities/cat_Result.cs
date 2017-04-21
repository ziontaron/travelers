using Reusable;

namespace BusinessSpecificLogic.EF
{
    public partial class cat_Result : BaseEntity
    {
        public override int id
        {
            get
            {
                return ResultKey;
            }
        }
    }
}
