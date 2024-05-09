using Labb4MvcAndRazor.Data;
using Labb4MvcAndRazor.Models;
using Labb4MvcAndRazor.Models.View_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Labb4MvcAndRazor.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchInput, int pageNumber = 1, int pageSize = 10)
        {
            var books = await _context.Books
                .OrderBy(b => b.Author)
                .ThenBy(b => b.Title)
                .Include(l => l.Loans)
                .ToListAsync();

            if (books == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(searchInput))
            {
                books = books.Where(b => b.Title.ToLower().Contains(searchInput.ToLower()) ||
                    b.Author.ToLower().Contains(searchInput.ToLower())).ToList();
            }

            int totalBooks = books.Count;
            int totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);

            if (pageNumber > totalPages)
            {
                return RedirectToAction(nameof(Index), new { pageNumber = totalPages });
            }

            books = books.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewData["totalPages"] = totalPages;
            ViewData["currentPage"] = pageNumber;

            return View(books);
        }

        public async Task<IActionResult> Details(int bookId)
        {
            var book = await _context.Books
                .Include(l => l.Loans)
                .ThenInclude(l => l.Customer)
                .FirstOrDefaultAsync(b => b.BookId == bookId);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        public async Task<IActionResult> MakeALoan(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
            {
                return BadRequest();
            }

            var viewModel = new MakeALoanViewModel(book);
            var customers = await _context.Customers.ToListAsync();
            ViewBag.Customers = new SelectList(customers, "CustomerId", "Name");

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeALoan(MakeALoanViewModel viewModel)
        {
            if (!ModelState.IsValid || !_context.Customers.Any(c => c.CustomerId == viewModel.CustomerId))
            {
                var customers = await _context.Customers.ToListAsync();
                viewModel.Book = await _context.Books.FindAsync(viewModel.BookId);
                ViewBag.Customers = new SelectList(customers, "CustomerId", "Name");
                ModelState.AddModelError("CustomerId", "Please select a customer");
                return View(viewModel);
            }

            try
            {
                var loan = new Loan
                {
                    FkBookId = viewModel.BookId,
                    FkCustomerId = viewModel.CustomerId,
                    LoanDate = viewModel.LoanDate,
                    LoanStatus = LoanStatus.Loaned,
                };

                _context.Loans.Add(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the loan. Please try again.");

                var customers = await _context.Customers.ToListAsync();
                viewModel.Book = await _context.Books.FindAsync(viewModel.BookId);
                ViewBag.Customers = new SelectList(customers, "CustomerId", "Name");
                return View(viewModel);
            }
        }
    }
}
