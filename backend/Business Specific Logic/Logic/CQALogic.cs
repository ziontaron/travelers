using BusinessSpecificLogic.EF;
using Reusable;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Linq;

namespace BusinessSpecificLogic.Logic
{
    public interface ICQAHeaderLogic : IBaseLogic<CQAHeader>
    {

    }

    public class CQAHeaderLogic : BaseLogic<CQAHeader>, ICQAHeaderLogic
    {
        private readonly IRepository<Customer> customerRepository;

        public CQAHeaderLogic(DbContext context, IRepository<CQAHeader> repository,
            IRepository<Customer> customerRepository) : base(context, repository)
        {
            this.customerRepository = customerRepository;
        }

        protected override void loadNavigationProperties(params CQAHeader[] entities)
        {
            var ctx = context as CQAContext;
            foreach (var item in entities)
            {
                item.Customer = customerRepository.GetByID(item.CustomerKey ?? -1);
                item.CQANumber = ctx.CQANumbers.Where(n => n.CQANumberKey == item.CQANumberKey).FirstOrDefault();
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
            if (mode == OPERATION_MODE.ADD)
            {
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
            }
        }


        #region Catalogs
        protected override ICatalogContainer LoadCatalogs()
        {
            return new Catalogs()
            {
                Customer = customerRepository.GetAll()
            };
        }

        private class Catalogs : ICatalogContainer
        {
            public IList<Customer> Customer { get; set; }
        }
        #endregion

    }

}
