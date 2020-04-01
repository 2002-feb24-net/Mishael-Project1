using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace POne.Models
{
    public class ProductModel
    {
        [Display(Name = "This is not intended to be displayed")]
        public int? PrdId { get; set; }

        [Display(Name = "This is not intended to be displayed")]
        public int LocId { get; set; }
        
        [Required(ErrorMessage = "Product must have a name")]
        [Display(Name = "Item")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Product must have a price")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Product must have a quantity")]
        [Display(Name = "Quantity")]
        public int Stock { get; set; }
    }
}
