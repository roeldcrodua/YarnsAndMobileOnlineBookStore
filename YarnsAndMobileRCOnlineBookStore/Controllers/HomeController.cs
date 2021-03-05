using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YarnsAndMobileRCOnlineBookStore.Data;
using YarnsAndMobileRCOnlineBookStore.Models;

namespace YarnsAndMobileRCOnlineBookStore.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var books = from book in _context.Books select book;
            int pageSize;
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(book => book.Title.Contains(searchString)
                                       || book.AuthorFirstName.Contains(searchString)
                                       || book.AuthorLastName.Contains(searchString));
                pageSize = books.Count();
            }
            else
            {
                pageSize = 20;
            }
            return View(await PaginatedList<Book>.CreateAsync(books.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

            public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
