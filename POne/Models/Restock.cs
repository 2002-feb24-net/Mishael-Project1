using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace POne.Models
{
    public class Restock
    {
        [Display(Name = "Enter quantity:")]
        public int quantity { get; set }
    }
}
