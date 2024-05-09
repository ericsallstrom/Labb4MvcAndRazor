using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Labb4MvcAndRazor.Models
{
    public class Customer
    {
        [Key]
        [DisplayName("ID")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 200 characters")]
        [DisplayName("Name")]        
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Address must be between 2 and 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Zip code is required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Zip code must be between 2 and 30 characters")]
        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "City must be between 2 and 200 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(150, MinimumLength = 4, ErrorMessage = "Email must be between 4 and 250 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(25, ErrorMessage = "Phone number cannot have more than 25 characters")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public ICollection<Loan>? Loans { get; set; }
    }
}
