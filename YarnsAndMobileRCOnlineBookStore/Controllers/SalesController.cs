using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YarnsAndMobileRCOnlineBookStore.Data;
using YarnsAndMobileRCOnlineBookStore.Models;
using YarnsAndMobileRCOnlineBookStore.Models.Data;

namespace YarnsAndMobileRCOnlineBookStore.Controllers
{
    public partial class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Member> _userManager;
        private readonly SignInManager<Member> _signInManager;

        public SalesController(ApplicationDbContext context, UserManager<Member> userManager, SignInManager<Member> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public string MemberId { get; set; }
            [Display(Name = "Account Number")]
            public string AccountNumber { get; set; }

            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "Book Title")]
            public string Title { get; set; }

            [Display(Name = "Purchased Date")]

            public DateTime PurchaseDate { get; set; }
            public int BookId { get; set; }

            public decimal Price { get; set; }
        }
            // GET: Sales
            public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewBag.CurentSort = sortOrder;
            ViewBag.TitleSort = String.IsNullOrEmpty(sortOrder) ? "Title" : "PurchaseDate";
            ViewBag.MemberSort = sortOrder == "Email" ? "Title" : "Email";
            ViewBag.BookSort = sortOrder == "Title" ? "Email" : "Title";
            ViewBag.PriceSort = sortOrder == "Price" ? "PurchaseDate" : "Price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var sales = from sale in _context.Sales select sale;
            sales = sales.Include(c => c.Books).Include(m => m.Members);
            if (!String.IsNullOrEmpty(searchString))
            {
                sales = sales.Where(sale => sale.Books.Title.Contains(searchString)
                                       || sale.Members.Email.Contains(searchString)
                                       || sale.Members.UserName.Contains(searchString));

            }
            
            switch (sortOrder)
            {
                case "Title":
                    sales = sales.OrderBy(s => s.Books.Title);
                    //sales = (IQueryable<Sale>)(Sale)bookTitles;
                    break;
                case "Email":
                    sales = sales.OrderBy(s => s.Members.Email);
                    break;
                case "Price":
                    sales = sales.OrderByDescending(s => s.Price);
                    break;
                default:
                    sales = sales.OrderBy(s => s.PurchaseDate);
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<Sale>.CreateAsync(sales.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales.Include(m=>m.Members).Include(b=>b.Books).FirstOrDefaultAsync(m => m.OrderId == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // GET: Sales/Create
        public async Task<IActionResult> Create(int id)
        {
            var book = _context.Books.Find(id);
            if (_signInManager.IsSignedIn(User))
            {
                var member = await _userManager.GetUserAsync(User);

                Input = new InputModel
                {
                    PurchaseDate = DateTime.Now.Date,
                    Price = book?.SalePrice ?? 0,
                    Title = book.Title,
                    BookId = book.BookId,
                    AccountNumber = member.AccountNumber,
                    Email = member.Email

                };
                return View(Input);
            }
            else
            {

                return RedirectPreserveMethod("~/Identity/Account/Register");
            }
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(int id, Sale sale)
        {
            if (ModelState.IsValid)
            {

                var book = _context.Books.Find(id);
                var member = await _userManager.GetUserAsync(User);

                Input = new InputModel
                {
                    PurchaseDate = DateTime.Now.Date,
                    Price = book?.SalePrice ?? 0,
                    Title = book.Title,
                    BookId = book.BookId,
                    AccountNumber = member.AccountNumber,
                    Email = member.Email

                };

                sale.PurchaseDate = Input.PurchaseDate;
                sale.Price = Input.Price;
                sale.Members = member;
                sale.Books = book;

                _context.Add(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sale);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales.Include(m => m.Members).Include(b => b.Books).FirstOrDefaultAsync(m => m.OrderId == id);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,PurchaseDate,Price")] Sale sale)
        {
            if (id != sale.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sale);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales.Include(m => m.Members).Include(b => b.Books).FirstOrDefaultAsync(m => m.OrderId == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
            return _context.Sales.Any(e => e.OrderId == id);
        }

    }
}
