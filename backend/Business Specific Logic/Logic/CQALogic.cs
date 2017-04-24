using BusinessSpecificLogic.EF;
using Reusable;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Linq;
using BusinessSpecificLogic.FS.Customer;
using BusinessSpecificLogic.FS;

namespace BusinessSpecificLogic.Logic
{
    public interface ICQAHeaderLogic : IBaseLogic<CQAHeader>
    {

    }

    public class CQAHeaderLogic : BaseLogic<CQAHeader>, ICQAHeaderLogic
    {
        private readonly FSReadOnlyRepository<FSCustomer> customerRepository;
        private readonly FSReadOnlyRepository<FSItem> itemRepository;
        private readonly Repository<User> userRepository;
        private readonly Repository<cat_Status> statusRepository;

        public CQAHeaderLogic(DbContext context, IRepository<CQAHeader> repository,
            FSReadOnlyRepository<FSCustomer> customerRepository,
            FSReadOnlyRepository<FSItem> itemRepository,
            Repository<User> userRepository,
            Repository<cat_Status> statusRepository) : base(context, repository)
        {
            this.customerRepository = customerRepository;
            this.itemRepository = itemRepository;
            this.userRepository = userRepository;
            this.statusRepository = statusRepository;
        }

        protected override void loadNavigationProperties(params CQAHeader[] entities)
        {
            var ctx = context as CQAContext;
            foreach (var item in entities)
            {
                item.Customer = customerRepository.GetByID(item.CustomerKey ?? -1);
                item.CustomerValue = item.Customer != null ? item.Customer.Value : "";
                item.FSItem = itemRepository.GetByID(item.PartNumberKey ?? -1);
                item.CQANumber = ctx.CQANumbers.Where(n => n.CQANumberKey == item.CQANumberKey).FirstOrDefault();

                var concern = ctx.cat_ConcernType.Where(e => e.ConcernTypeKey == item.ConcernTypeKey).FirstOrDefault();
                var result = ctx.cat_Result.Where(e => e.ResultKey == item.ResultKey).FirstOrDefault();
                var status = ctx.cat_Status.Where(e => e.StatusKey== item.StatusKey).FirstOrDefault();
                item.ConcernValue = concern != null ? concern.Value + " - " + item.ConcertDescription : item.ConcertDescription;
                item.ResultValue = result != null ? result.Value : "";
                item.StatusValue = status != null ? status.Value : "";
                item.CQANumberValue = item.CQANumber.GeneratedNumber;  
            }
        }

        public override List<Expression<Func<CQAHeader, object>>> NavigationPropertiesWhenGetAll
        {
            get
            {
                return new List<Expression<Func<CQAHeader, object>>>()
                {
                    e => e.CQANumber
                };
            }
        }

        protected override void onCreate(CQAHeader entity)
        {
            base.onCreate(entity);
            entity.NotificationDate = DateTime.Now;
        }

        protected override void onBeforeSaving(CQAHeader entity, BaseEntity parent = null, OPERATION_MODE mode = OPERATION_MODE.NONE)
        {
            if (entity.FSItem != null)
            {
                entity.PartNumberKey = entity.FSItem.id;
            }

            if (mode == OPERATION_MODE.ADD)
            {
                #region CQA Number Generation
                var ctx = context as CQAContext;

                DateTime date = DateTime.Now;

                int sequence = 0;
                var last = ctx.CQANumbers.Where(n => n.CreatedDate.Year == date.Year
                && n.CreatedDate.Month == date.Month && n.CreatedDate.Day == date.Day).OrderByDescending(n => n.Sequence).FirstOrDefault();

                if (last != null)
                {
                    sequence = last.Sequence + 1;
                }

                string generated = date.Year.ToString().Substring(2) + date.Month.ToString("D2") + date.Day.ToString("D2") + sequence.ToString("D3");


                CQANumber cqaNumber = ctx.CQANumbers.Add(new CQANumber()
                {
                    CreatedDate = date,
                    Sequence = sequence,
                    GeneratedNumber = generated,
                    Revision = "A"
                });

                ctx.SaveChanges();

                entity.CQANumberKey = cqaNumber.CQANumberKey;
                #endregion
            }
        }


        #region Catalogs
        protected override ICatalogContainer LoadCatalogs()
        {
            return new Catalogs()
            {
                Customer = customerRepository.GetAll(),
                User = userRepository.GetAll(),
                Status = statusRepository.GetAll()
            };
        }

        private class Catalogs : ICatalogContainer
        {
            public IList<FSCustomer> Customer { get; set; }
            public IList<User> User { get; set; }
            public IList<cat_Status> Status { get; set; }
        }
        #endregion

    }

}
