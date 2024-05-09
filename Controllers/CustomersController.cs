using Labb4MvcAndRazor.Data;
using Labb4MvcAndRazor.Models;
using Labb4MvcAndRazor.Models.View_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Labb4MvcAndRazor.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllCustomers(string searchInput, int pageNumber = 1, int pageSize = 10)
        {
            var customers = await _context.Customers.OrderBy(c => c.Name).ToListAsync();

            if (customers == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(searchInput))
            {
                customers = customers.Where(c => c.Name.ToLower().Contains(searchInput.ToLower()) ||
                    c.Email.Contains(searchInput.ToLower()) || c.City.ToLower().Contains(searchInput.ToLower())).ToList();
            }

            int totalCustomers = customers.Count;
            int totalPages = (int)Math.Ceiling((double)totalCustomers / pageSize);

            if (pageNumber > totalPages)
            {
                return RedirectToAction(nameof(GetAllCustomers), new { pageNumber = totalPages });
            }

            customers = customers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewData["totalPages"] = totalPages;
            ViewData["currentPage"] = pageNumber;

            return View(customers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetAllCustomers));
            }

            return View(customer);
        }

        public async Task<IActionResult> Details(int customerId)
        {
            var customer = await _context.Customers
                .Include(c => c.Loans.OrderByDescending(l => l.LoanDate))
                .ThenInclude(l => l.Book)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return BadRequest();
            }

            return View(customer);
        }

        public async Task<IActionResult> ReturnBook(int customerId)
        {
            var customer = await _context.Customers
                 .Include(c => c.Loans.Where(l => l.LoanStatus != LoanStatus.Returned))
                 .ThenInclude(l => l.Book)
                 .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return NotFound();
            }

            var books = customer.Loans.Select(l => l.Book).ToList();
            ViewBag.Books = new SelectList(books, "BookId", "Title");

            var viewModel = new ReturnBookViewModel
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.Name,                                           
            };

            return View(viewModel);     
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnBook(ReturnBookViewModel viewModel)
        {
            if (!ModelState.IsValid || !_context.Books.Any(b => b.BookId == viewModel.BookId))
            {                
                ModelState.AddModelError("BookId", "Please select a book");
                return View(viewModel);
            }

            try
            {
                var loan = await _context.Loans
                    .Where(c => c.FkCustomerId == viewModel.CustomerId)
                    .FirstOrDefaultAsync(l => l.FkBookId == viewModel.BookId);

                if (loan == null)
                {
                    return NotFound();
                }

                loan.ReturnDate = viewModel.ReturnDate;
                loan.LoanStatus = LoanStatus.Returned;

                _context.Update(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), "Customers", new { customerId = loan.FkCustomerId });                
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError($"{ex}", "An error occurred while creating the loan. Please try again.");

                return View(viewModel);
            }
        }

        public async Task<IActionResult> Edit(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (!CustomerExists(customerId))
            {
                return BadRequest();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int customerId, Customer customer)
        {
            if (customerId != customer.CustomerId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Customers.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (!CustomerExists(customerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw new Exception($"Error: {ex.Message}");
                    }
                }

                return RedirectToAction(nameof(GetAllCustomers));
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
            {
                return NotFound();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var loansToDelete = _context.Loans.Where(l => l.FkCustomerId == customerId).ToList();

                    foreach (var loan in loansToDelete)
                    {
                        _context.Loans.Remove(loan);
                    }

                    _context.Customers.Remove(customer);

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return RedirectToAction(nameof(GetAllCustomers));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(c => c.CustomerId == id);
        }
    }
}
