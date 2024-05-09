using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Labb4MvcAndRazor.Models
{
    public class Loan
    {
        [Key]
        public int LoanId { get; set; }

        [ForeignKey(nameof(Book))]
        public int FkBookId { get; set; }
        public Book Book { get; set; }

        [ForeignKey(nameof(Customer))]
        public int FkCustomerId { get; set; }
        public Customer Customer { get; set; }

        [DisplayName("Loaned Date")]
        [Required(ErrorMessage = "Loan date is required")]        
        public DateTime LoanDate { get; set; }

        [DisplayName("Due")]
        public DateTime DueDate => LoanDate.AddDays(21);

        [DisplayName("Returned Date")]
        public DateTime? ReturnDate { get; set; }

        public LoanStatus LoanStatus { get; set; } = LoanStatus.Available;

        public bool IsLoaned => LoanStatus == LoanStatus.Loaned;
        public bool IsReturned => LoanStatus == LoanStatus.Returned;
        public bool IsLate => LoanStatus == LoanStatus.Loaned && DateTime.Today > DueDate;
    }

    public enum LoanStatus
    {
        Available,
        Loaned,
        Returned,
        Late
    }
}
