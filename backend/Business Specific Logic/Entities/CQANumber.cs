using Reusable;

namespace BusinessSpecificLogic.EF
{
    public partial class CQANumber : BaseEntity
    {
        public override int id
        {
            get
            {
                return CQANumberKey;
            }
        }
    }
}
