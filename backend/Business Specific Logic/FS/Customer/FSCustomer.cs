using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessSpecificLogic.FS.Customer
{
    public class FSCustomer
    {
        public int CustomerKey { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Value
        {
            get
            {
                return CustomerID + " " + CustomerName;
            }
        }
    }
}
