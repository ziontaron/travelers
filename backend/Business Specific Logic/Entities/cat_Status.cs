using Reusable;

namespace BusinessSpecificLogic.EF
{
    public partial class cat_Status : BaseEntity
    {
        public override int id
        {
            get
            {
                return StatusKey;
            }
        }
    }
}
