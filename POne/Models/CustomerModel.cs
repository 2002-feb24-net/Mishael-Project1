using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace POne.Models
{
    public class CustomerModel
    {
        [DisplayName("This is not intended to be displayed")]
        public int? CustId { get; set; }
        
        [Required(ErrorMessage = "First Name is Required")]
        [DisplayName("First Name")]
        [RegularExpression(@"^[a-zA-Z\s]{1,40}$", ErrorMessage = "Invalid characters")]
        public string FName { get; set; }
        
        [DisplayName("Last Name")]
        [RegularExpression(@"^[a-zA-Z\s]{1,40}$", ErrorMessage = "Invalid characters")]
        public string LName { get; set; }
    }
}
