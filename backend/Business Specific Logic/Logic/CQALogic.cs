using BusinessSpecificLogic.EF;
using Reusable;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;

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

        protected override void loadNavigationProperties(DbContext context, params CQAHeader[] entities)
        {
            foreach (var item in entities)
            {
                item.Customer = customerRepository.GetByID(item.CustomerKey ?? -1);
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
            entity.CQANumberKey = 2;
            entity.PartNumberKey = 1;
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
