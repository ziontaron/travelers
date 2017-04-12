using Reusable;

namespace BusinessSpecificLogic.FS.Customer
{
    public interface IFSItemLogic : IReadOnlyBaseLogic<FSItem> { }

    public class FSItemLogic : ReadOnlyBaseLogic<FSItem>, IFSItemLogic
    {
        public FSItemLogic(FSContext context, IReadOnlyRepository<FSItem> repository) : base(context, repository)
        {
        }
    }
}
