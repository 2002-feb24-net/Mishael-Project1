using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace POne.Models
{
    public class LocationModel
    {
        [Display(Name = "This is not intended to be displayed")]
        public int? LocID { get; set; }
        
        [Required(ErrorMessage = "Location must have a name")]
        [Display(Name = "Store")]
        public string Name { get; set; }
    }
}
