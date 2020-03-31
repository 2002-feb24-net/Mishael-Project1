using System;
using System.Collections.Generic;

namespace PZero.dtb.Entities
{
    public partial class Orders
    {
        public Orders()
        {
            OrderData = new HashSet<OrderData>();
        }

        public int OrdId { get; set; }
        public int CustId { get; set; }
        public decimal Total { get; set; }
        public DateTime? Stamp { get; set; }

        public virtual Customers Cust { get; set; }
        public virtual ICollection<OrderData> OrderData { get; set; }
    }
}
