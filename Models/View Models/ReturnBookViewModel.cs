using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;

namespace Labb4MvcAndRazor.Models.View_Models
{
    public class ReturnBookViewModel
    {
        public int LoanId { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }              
        public DateTime ReturnDate { get; } = DateTime.Now;
        public LoanStatus LoanStatus { get; set; }        
    }
}
