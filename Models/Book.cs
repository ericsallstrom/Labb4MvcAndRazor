using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Labb4MvcAndRazor.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]        
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "The author's name must be between 1 and 200 characters")]
        [DisplayName("Author")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Genre must be between 1 and 200 characters")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(400, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 400 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ISBN is required")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 13 characters")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Please enter a publication date")]
        [DisplayName("Publication date")]
        public DateTime PublicationDate { get; set; }

        [Required(ErrorMessage = "Publisher is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "The name of the publisher must be between 1 and 200 characters")]
        public string Publisher { get; set; }

        [Required(ErrorMessage = "Please enter the number of pages")]
        [DisplayName("Number of pages")]
        public int NumberOfPages { get; set; }

        [Required(ErrorMessage = "Language is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "The entered language must be between 2 and 150 characters")]
        public string Language { get; set; }

        public ICollection<Loan>? Loans { get; set; }
    } 
}
