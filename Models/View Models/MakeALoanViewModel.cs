using System.ComponentModel.DataAnnotations;

namespace Labb4MvcAndRazor.Models.View_Models
{
    public class MakeALoanViewModel
    {
        public MakeALoanViewModel()
        {            
        }

        public MakeALoanViewModel(Book book)
        {
            BookId = book.BookId;
            Book = book;
        }

        public int CustomerId { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }        
        public DateTime LoanDate { get; set; } = DateTime.Now;
        public DateTime DueDate => LoanDate.AddDays(21);
        public LoanStatus LoanStatus { get; set; }
    }
}
