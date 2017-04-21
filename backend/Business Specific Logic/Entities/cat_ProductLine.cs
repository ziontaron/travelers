using Reusable;

namespace BusinessSpecificLogic.EF
{
    public partial class cat_ProductLine : BaseEntity
    {
        public override int id
        {
            get
            {
                return ProductLineKey;
            }
        }
    }
}
