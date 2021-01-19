using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using YarnsAndMobileRCOnlineBookStore.Data;
using YarnsAndMobileRCOnlineBookStore.Models.Data;
using Newtonsoft.Json;
using YarnsAndMobileRCOnlineBookStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace YarnsAndMobileRCOnlineBookStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Member> _userManager;
        private readonly IWebHostEnvironment _env;

        public AdminController(ApplicationDbContext dbContext, UserManager<Member> userManager, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _env = environment;
        }
        public IActionResult Index()
        {
            ViewData["message"] = TempData["message"];
            return View();
        }

        public IActionResult ImportData()
        {
            var userAdmin = _userManager.GetUserId(User);
            ApplicationDbInitializer.SeedUsers(_userManager, userAdmin);

            var bookKeyMap = LoadBooks();
            var memberKeyMap = LoadMembers();
            LoadSalesAndReviews(bookKeyMap, memberKeyMap);

            return RedirectToAction(nameof(Index));
        }

        private Dictionary<int, int> LoadMembers()
        {
            var jsonFile = System.IO.File.ReadAllText(@"C:\Users\Roel\Documents\MCSD\PROJECT\YarnsAndMobileRCOnlineBookStore\Yarns_Member.json");
            var importMembers = JsonConvert.DeserializeObject<List<Member>>(jsonFile);

            var keyValue = new Dictionary<int, int>();
            foreach (var importMember in importMembers)
            {
                var address = new Address
                {
                    Street = $"{importMember.Addresses.Line1} {importMember.Addresses.Line2}",
                    City = importMember.Addresses.City,
                    State = importMember.Addresses.State,
                    Zip = importMember.Addresses.Zip
                };

                var phone = new Phone
                {
                    Phone1 = importMember.PhoneNumbers.Phone1,
                    Phone2 = importMember.PhoneNumbers.Phone2,
                    Phone3 = importMember.PhoneNumbers.Phone3,
                };

                var member = new Member
                {
                    AccountNumber = importMember.AccountNumber,
                    FirstName = importMember.FirstName,
                    LastName = importMember.LastName,
                    Email = importMember.Email
                };

                member.PhoneNumbers = phone;
                member.Addresses = address;
                keyValue.Add(importMember.MemberId, member.MemberId);

            }
            _dbContext.SaveChanges();
            return keyValue;
        }

        private Dictionary<int, int> LoadBooks()
        {
            var jsonFile = System.IO.File.ReadAllText(@"C:\Users\Roel\Documents\MCSD\PROJECT\YarnsAndMobileRCOnlineBookStore\Yarns_Book.json");
            var importBooks = JsonConvert.DeserializeObject<List<Book>>(jsonFile);

            var keyValue = new Dictionary<int, int>();
            foreach (var importBook in importBooks)
            {
                var book = new Book
                {
                    AuthorFirstName = importBook.AuthorFirstName,
                    AuthorLastName = importBook.AuthorLastName,
                    CopyrightYear = (short)importBook.CopyrightYear.GetValueOrDefault(),
                    Image = importBook.Image,
                    ISBN = importBook.ISBN,
                    SalePrice = importBook.SalePrice,
                    Title = importBook.Title
                };
                _dbContext.Books.Add(book);
                _dbContext.SaveChanges();
                keyValue.Add(importBook.BookId, book.BookId);
            }
            _dbContext.SaveChanges();
            return keyValue;
        }

        private void LoadSalesAndReviews(Dictionary<int, int> bookKeyMap, Dictionary<int, int> memberKeyMap)
        {
            var jsonFile = System.IO.File.ReadAllText(@"C:\Users\Roel\Documents\MCSD\PROJECT\YarnsAndMobileRCOnlineBookStore\Yarns_SaleReview.json");
            var importSaleReviews = JsonConvert.DeserializeObject<List<Review>>(jsonFile);

            bookKeyMap = LoadBooks();
            memberKeyMap = LoadMembers();

            foreach (var importSaleReview in importSaleReviews)
            {
                var book = _dbContext.Books.Find(bookKeyMap[importSaleReview.Books.BookId]);
                var member = _dbContext.Members.Find(memberKeyMap[importSaleReview.Members.MemberId]);

                if (importSaleReview.SaleDate.HasValue)
                {
                    var sale = new Sale
                    {
                        PurchaseDate = importSaleReview.SaleDate.GetValueOrDefault(),
                        Price = importSaleReview.SalePrice.GetValueOrDefault(),
                        Books = book,
                        Members = member
                    };
                    _dbContext.Sales.Add(sale);
                }
                if (importSaleReview.ReviewDate.HasValue)
                {
                    var review = new Review
                    {
                        ReviewDate = importSaleReview.ReviewDate.GetValueOrDefault(),
                        Title = importSaleReview.Title,
                        Text = importSaleReview.Text,
                        StarRating = importSaleReview.StarRating,
                        Books = book,
                        Members = member
                    };
                    _dbContext.Reviews.Add(review);
                }
                _dbContext.SaveChanges();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, string fileDataType)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    TempData["message"] = "File not selected";
                    return RedirectToAction(nameof(Index));
                }

                if (Path.GetExtension(file.Name).ToLower() != ".json")
                {
                    TempData["message"] = "Only json files are supported at this time";
                    return RedirectToAction(nameof(Index));
                }

                var path = Path.GetFullPath(_env.ApplicationName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch
            {
                TempData["message"] = "An unexpected error occurred";
                return RedirectToAction(nameof(Index));
            }

            TempData["message"] = "File Uploaded";
            return RedirectToAction(nameof(Index));
        }

    }
}
