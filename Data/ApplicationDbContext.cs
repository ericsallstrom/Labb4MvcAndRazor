using Labb4MvcAndRazor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Labb4MvcAndRazor.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            var customers = new[]
            {
                new Customer
                {
                    CustomerId = 1,
                    Name = "Eric Sällström",
                    Address = "Tegelvägen 3C",
                    ZipCode = "791 35",
                    City = "Falun",
                    Email = "eric.sallstrom@gmail.com",
                    PhoneNumber = "0725522355"
                },
               new Customer
                {
                   CustomerId = 2,
                    Name = "Wilma Andersson",
                    Address = "Storgatan 12",
                    ZipCode = "123 45",
                    City = "Lund",
                    Email = "wilma.andersson@gmail.com",
                    PhoneNumber = "0701234567"
                },
                new Customer
                {
                    CustomerId = 3,
                    Name = "Anna Johansson",
                    Address = "Lillgatan 8",
                    ZipCode = "543 21",
                    City = "Stockholm",
                    Email = "anna.johansson@gmail.com",
                    PhoneNumber = "0709876543"
                }
            };

            var books = new[]
            {
               new Book
               {
                   BookId = 1,
                   Title = "On the Road",
                   Author = "Jack Kerouac",
                   Genre = "Novel",
                   Description = "\"On the Road\" by Jack Kerouac is a quintessential novel of the Beat Generation, chronicling the travels of " +
                   "Jack (Sal Paradise) and his friend Dean Moriarty across North America. Their journey represents a quest for self-discovery " +
                   "and experience, infused with Kerouac's love for America and his unique, jazz-like language.",
                   ISBN = "9780140042597",
                   Language = "English",
                   NumberOfPages = 307,
                   PublicationDate = new DateTime(1957, 09, 05),
                   Publisher = "Viking Press Inc."
               },
               new Book
               {
                   BookId = 2,
                   Title = "Oranges Are Not The Only Fruit",
                   Author = "Jeanette Winterson",
                   Genre = "Fiction",
                   Description = "\"Orange Is Not the Only Fruit\" by Jeanette Winterson is a semi-autobiographical novel about a young girl " +
                   "named Jeanette growing up in a strict evangelical household in England. The story explores themes of identity, religion, and " +
                   "sexuality as Jeanette grapples with her own desires and beliefs.",
                   ISBN = "9780099598183",
                   Language = "English",
                   NumberOfPages = 224,
                   PublicationDate = new DateTime(1985, 03, 21),
                   Publisher = "Pandora Press"
               },
               new Book
               {
                   BookId = 3,
                   Title = "Interpreter of Maladies",
                   Author = "Jhumpa Lahiri",
                   Genre = "Short Stories",
                   Description = "\"Interpreter of Maladies\" by Jhumpa Lahiri is a collection of short stories that delves into the lives of " +
                   "Indian immigrants and their descendants, navigating cultural displacement, identity, and the complexities of human " +
                   "relationships. Lahiri's exquisite prose crafts intimate portraits of characters grappling with loneliness, longing, " +
                   "and the search for connection across borders and generations.",
                   ISBN = "9780618101368",
                   Language = "English",
                   NumberOfPages = 198,
                   PublicationDate = new DateTime(1999, 04, 20),
                   Publisher = "Flamingo"
               },
               new Book
               {
                   BookId = 4,
                   Title = "Jerusalem #1-2",
                   Author = "Selma Lagerlöf",
                   Genre = "Fiction",
                   Description = "\"Jerusalem\" by Selma Lagerlöf is an epic novel set in late 19th-century Sweden, intertwining the lives of " +
                   "various characters in the rural village of Jerusalem. The narrative unfolds against the backdrop of social upheaval and " +
                   "spiritual awakening, exploring themes of redemption, forgiveness, and the struggle for justice.",
                   ISBN = "9781426489228",
                   Language = "English",
                   NumberOfPages = 582,
                   PublicationDate = new DateTime(1901, 01, 01),
                   Publisher = "Albert Bonniers Förlag"
               }
            };

            var loans = new[]
            {
                new Loan
                {
                    LoanId = 1,
                    FkBookId = 4,
                    FkCustomerId = 1,
                    LoanDate = new DateTime(2024, 04, 25),
                    LoanStatus = LoanStatus.Loaned
                },
                new Loan
                {
                    LoanId = 2,
                    FkBookId = 3,
                    FkCustomerId = 2,
                    LoanDate = new DateTime(2024, 01, 09),
                    ReturnDate = new DateTime(2024, 02, 23),
                    LoanStatus = LoanStatus.Returned
                },
                new Loan
                {
                    LoanId = 3,
                    FkBookId = 1,
                    FkCustomerId = 3,
                    LoanDate = new DateTime(2024, 02, 19),
                    LoanStatus = LoanStatus.Loaned
                }
            };

            modelBuilder.Entity<Customer>().HasData(customers);
            modelBuilder.Entity<Book>().HasData(books);
            modelBuilder.Entity<Loan>().HasData(loans);
        }
    }
}
