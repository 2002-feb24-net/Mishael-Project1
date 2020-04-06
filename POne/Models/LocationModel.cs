using POne.Lib;
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
        [RegularExpression(@"^[a-zA-Z \s]{1,40}$", ErrorMessage = "Invalid characters")]
        public string Name { get; set; }

        public bool Removable { get => Output.IsRemovableLocation(LocID); }
    }
}
