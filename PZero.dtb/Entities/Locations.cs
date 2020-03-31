using System;
using System.Collections.Generic;

namespace PZero.dtb.Entities
{
    public partial class Locations
    {
        public Locations()
        {
            Products = new HashSet<Products>();
        }

        public int LocId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
