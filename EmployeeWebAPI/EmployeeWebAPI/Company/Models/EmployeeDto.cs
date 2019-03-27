using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Company.Models
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Gender { get; set; }
        [Display(Name="Email Id")]
        [Required]
        public string EmailId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Display(Name = "Contact Number")]     
        [DataType(DataType.PhoneNumber)]  
        [Required(ErrorMessage = "Phone Number Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string ContactNumber { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public List<string> Genders { get; set; }
    }
}