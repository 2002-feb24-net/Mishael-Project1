using System;
using System.Collections.Generic;

namespace PZero.dtb.Entities
{
    public partial class OrderData
    {
        public int DataId { get; set; }
        public int OrdId { get; set; }
        public int PrdId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public virtual Orders Ord { get; set; }
        public virtual Products Prd { get; set; }
    }
}
