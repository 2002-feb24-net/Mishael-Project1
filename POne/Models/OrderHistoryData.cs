using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace POne.Models
{
    public class OrderHistoryData
    {
        [Display(Name = "Product Name")]
        [RegularExpression(@"^[a-zA-Z \s]{1,40}$", ErrorMessage = "Invalid characters")]
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Date Placed")]
        public DateTime Stamp { get; set; }
        [DataType(DataType.Currency)]
        public decimal Total => Price * Quantity;
    }
}
