﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YarnsAndMobileRCOnlineBookStore.Data;
using YarnsAndMobileRCOnlineBookStore.Models;
using YarnsAndMobileRCOnlineBookStore.Models.Data;

namespace YarnsAndMobileRCOnlineBookStore.Controllers
{
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Member> _userManager;

        public SalesController(ApplicationDbContext context, UserManager<Member> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                                       || sale.Books.Title.Contains(searchString)
                                       || sale.Price.Equals(decimal.Parse(searchString))
                                       || sale.PurchaseDate.Equals(DateTime.Parse(searchString).ToShortDateString()));
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

            var sale = await _context.Sales
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }

        // GET: Sales/Create
        public IActionResult Create(int id)
        {
            var book = _context.Books.Find(id);
            var sale = new Sale
            {
                Books = book,
                Price = book?.SalePrice ?? 0,
                PurchaseDate = DateTime.Now,
                Members = _userManager.GetUserAsync(User).Result
            };
            return View(sale);
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sale sale)
        {
            if (ModelState.IsValid)
            {
                sale.Members = _context.Members.Find(_userManager.GetUserAsync(User));
                sale.Books = _context.Books.Find(sale.Books.BookId);
                sale.OrderId = 0;
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

            var sale = await _context.Sales.FindAsync(id);
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

            var sale = await _context.Sales
                .FirstOrDefaultAsync(m => m.OrderId == id);
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
