using System;
using System.Collections.Generic;

namespace PZero.dtb.Entities
{
    public partial class Products
    {
        public Products()
        {
            OrderData = new HashSet<OrderData>();
        }

        public int PrdId { get; set; }
        public int LocId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public virtual Locations Loc { get; set; }
        public virtual ICollection<OrderData> OrderData { get; set; }
    }
}
