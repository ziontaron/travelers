using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace BusinessSpecificLogic.FS.Customer
{
    public class FSCustomer : FSEntity
    {
        [NotMapped]
        public override int id { get { return CustomerKey; } }

        [NotMapped]
        public string Value { get { return CustomerID + " - " + CustomerName; } }

        public int CustomerKey { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }

        public override string sqlGetAll
        {
            get
            {
                return @"SELECT DISTINCT _NoLock_FS_Customer.CustomerKey
	                        ,_NoLock_FS_Customer.CustomerID
	                        ,_NoLock_FS_Customer.CustomerName
                        FROM _NoLock_FS_Customer
                        INNER JOIN _NoLock_FS_COHeader ON _NoLock_FS_Customer.CustomerKey = _NoLock_FS_COHeader.CustomerKey
                        INNER JOIN _NoLock_FS_COLine ON _NoLock_FS_COHeader.COHeaderKey = _NoLock_FS_COLine.COHeaderKey";
            }
        }

        public override string sqlGetById
        {
            get
            {
                return @"SELECT DISTINCT _NoLock_FS_Customer.CustomerKey
	                        ,_NoLock_FS_Customer.CustomerID
	                        ,_NoLock_FS_Customer.CustomerName
                        FROM _NoLock_FS_Customer
                        INNER JOIN _NoLock_FS_COHeader ON _NoLock_FS_Customer.CustomerKey = _NoLock_FS_COHeader.CustomerKey
                        INNER JOIN _NoLock_FS_COLine ON _NoLock_FS_COHeader.COHeaderKey = _NoLock_FS_COLine.COHeaderKey
                        WHERE _NoLock_FS_Customer.CustomerKey = @id";
            }
        }
    }
}
