using POne.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POne.Models
{
    public class CartItem
    {
        public int cartIndex { get; set; }
        public int ID { get; set; }
        public string ItemName => Output.GetProductName(ID);
        public int Quantity { get; set; }
        public decimal Price => Output.GetProductPrice(ID);
        public decimal Total => Price * Quantity;
    }
}
