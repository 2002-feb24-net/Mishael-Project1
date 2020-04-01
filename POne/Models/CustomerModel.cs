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
        public string FName { get; set; }
        
        [DisplayName("Last Name")]
        public string LName { get; set; }
    }
}
