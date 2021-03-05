using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YarnsAndMobileRCOnlineBookStore.Data;
using YarnsAndMobileRCOnlineBookStore.Models;
using YarnsAndMobileRCOnlineBookStore.Models.Data;
using YarnsAndMobileRCOnlineBookStore.Views.Admin;

namespace YarnsAndMobileRCOnlineBookStore.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Member> _userManager;
        private readonly SignInManager<Member> _signInManager;
        public ReviewsController(ApplicationDbContext context, UserManager<Member> userManager, SignInManager<Member> signInManager)
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

            [Display(Name = "Member Email")]
            public string Email { get; set; }

            [Display(Name = "Book Title")]
            public string BookTitle { get; set; }

            [Display(Name = "Review Title")]
            public string ReviewTitle { get; set; }
            [Display(Name = "Review Text")]
            public string ReviewText { get; set; }

            [Display(Name = "Review Date")]

            public DateTime ReviewDate { get; set; }
            public int BookId { get; set; }
            public int StarRating { get; set; }
            
        }
        // GET: Reviews
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewBag.CurentSort = sortOrder;
            ViewBag.TitleSort = String.IsNullOrEmpty(sortOrder) ? "Title" : "";
            ViewBag.DateSort = sortOrder == "ReviewDate" ? "Email" : "ReviewDate";
            ViewBag.EmailSort = sortOrder == "Email" ? "ReviewDate" : "Email";
            ViewBag.StarSort = sortOrder == "StarRating" ? "ReviewDate" : "StarRating";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var reviews = from review in _context.Reviews select review;
            reviews = reviews.Include(c => c.Books).Include(m => m.Members);
            if (!String.IsNullOrEmpty(searchString))
            {
                reviews = reviews.Where(review => review.Books.Title.Contains(searchString)
                                       || review.Members.Email.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "Title":
                    reviews = reviews.OrderBy(r => r.Books.Title);
                    break;
                case "StarRating":
                    reviews = reviews.OrderByDescending(r => r.StarRating);
                    break;
                case "Email":
                    reviews = reviews.OrderByDescending(r => r.Members.Email);
                    break;
                default:
                    reviews = reviews.OrderBy(r => r.ReviewDate);
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<Review>.CreateAsync(reviews.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.Include(m => m.Members).Include(b => b.Books)
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public async Task<IActionResult> Create(int id)
        {
            var book = _context.Books.Find(id);
            if (_signInManager.IsSignedIn(User))
            {
                var member = await _userManager.GetUserAsync(User);

                var review = await _context.Reviews.Include(m => m.Members).Include(b => b.Books).FirstOrDefaultAsync(m => m.Members.Id == member.Id && m.Books.BookId == book.BookId);

                if (review != null)
                {
                    return RedirectToAction("Edit", new {id = review.ReviewId });
                }
                Input = new InputModel
                {
                    ReviewDate = DateTime.Now.Date,
                    ReviewTitle = "",
                    BookId = book.BookId,
                    BookTitle = book.Title,
                    ReviewText = "",
                    StarRating = 1,
                    MemberId = member.Id,
                    Email = member.Email

                };
                return View(Input);
            }
            else
            {
             
                return RedirectPreserveMethod("~/Identity/Account/Register");
            }
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("ReviewId,ReviewTitle,ReviewText,StarRating,ReviewDate")] Review review)
        {
            if (ModelState.IsValid)
            {
                var book = _context.Books.Find(id);
                var member = await _userManager.GetUserAsync(User);

                review.ReviewDate = DateTime.Now.Date;
                review.Members = member;
                review.Books = book;

                if (Input.ReviewTitle == "") { }
                else if (Input.ReviewTitle != review.Title)
                {
                    review.Title = Input.ReviewTitle;
                }
                if (Input.StarRating > 0) { }
                else if (Input.StarRating != review.StarRating)
                {
                    review.StarRating = Input.StarRating;
                }
                if (Input.ReviewText == "") { }
                else if (Input.ReviewText != review.Text)
                {
                    review.Text = Input.ReviewText;
                }
                
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.Include(m => m.Members).Include(b => b.Books).FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewId,Title,Text,StarRating,ReviewDate")] Review review)
        {
            if (id != review.ReviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewId))
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
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.Include(m => m.Members).Include(b => b.Books)
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewId == id);
        }
    }
}
