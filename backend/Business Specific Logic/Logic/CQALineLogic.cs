using BusinessSpecificLogic.EF;
using Reusable;
using Reusable.Attachments;
using System.Data.Entity;

namespace BusinessSpecificLogic.Logic
{
    public interface ICQALineLogic : IBaseLogic<CQALine> { }

    public class CQALineLogic : BaseLogic<CQALine>, ICQALineLogic
    {
        public CQALineLogic(DbContext context, IRepository<CQALine> repository) : base(context, repository) { }

        protected override void loadNavigationProperties(params CQALine[] entities)
        {
            foreach (var item in entities)
            {
                item.Attachments = AttachmentsIO.getAttachmentsFromFolder(item.AttachmentsFolder, "CQALineAttachments");
            }
        }

    }

}