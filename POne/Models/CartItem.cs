using POne.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace POne.Models
{
    public class CartItem
    {
        [DisplayName("This is not intended to be displayed")]
        public int cartIndex { get; set; }
        [DisplayName("This is not intended to be displayed")]
        public int ID { get; set; }
        [DisplayName("Item")]
        public string ItemName => Output.GetProductName(ID);
        [DisplayName("Ammount")]
        public int Quantity { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price => Output.GetProductPrice(ID);
        [DataType(DataType.Currency)]
        public decimal Total => Price * Quantity;
    }
}
