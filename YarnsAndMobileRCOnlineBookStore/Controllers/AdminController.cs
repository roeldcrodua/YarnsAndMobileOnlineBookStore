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
using YarnsAndMobileRCOnlineBookStore.Models.ImportModels;
using YarnsAndMobileRCOnlineBookStore.Views.Admin;

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

        [Route("Admin")]
        public void ImportData()
        {
            var userAdmin = _userManager.GetUserId(User);
            ApplicationDbInitializer.SeedUsers(_userManager, userAdmin);


            var bookKeyMap = LoadBooks();
            var memberKeyMap = LoadMembers();
            LoadSalesAndReviews(bookKeyMap, memberKeyMap);

        }

        private Dictionary<int, string> LoadMembers()
        {
            var jsonFile = System.IO.File.ReadAllText(@"C:\Users\Roel\Documents\MCSD\PROJECT\YarnsAndMobileRCOnlineBookStore\Yarns_Member.json");
            var importMembers = JsonConvert.DeserializeObject<List<MemberImport>>(jsonFile);

            var keyValue = new Dictionary<int, string>();
            foreach (var importMember in importMembers)
            {
                var address = new Address
                {
                    Line1 = importMember.Line1,
                    Line2 = importMember.Line2,
                    Street = importMember.Street,
                    City = importMember.City,
                    State = importMember.State,
                    Zip = importMember.Zip
                };

                var phone = new Phone
                {
                    Phone1 = importMember.Phone1,
                    Phone2 = importMember.Phone2,
                    Phone3 = importMember.Phone3,
                    Phone4 = importMember.Phone4,
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
                _dbContext.Members.Add(member);
                keyValue.Add(importMember.Id, member.Id);

            }
            _dbContext.SaveChanges();
            return keyValue;
        }

        private Dictionary<int, int> LoadBooks()
        {
            var jsonFile = System.IO.File.ReadAllText(@"C:\Users\Roel\Documents\MCSD\PROJECT\YarnsAndMobileRCOnlineBookStore\Yarns_Book.json");
            var importBooks = JsonConvert.DeserializeObject<List<BookImport>>(jsonFile);

            var keyValue = new Dictionary<int, int>();
            foreach (var importBook in importBooks)
            {
                var book = new Book
                {
                    AuthorFirstName = importBook.AuthorFirstName,
                    AuthorLastName = importBook.AuthorLastName,
                    CopyrightYear = importBook.Year,
                    Image = importBook.Image,
                    ISBN = importBook.ISBN,
                    SalePrice = importBook.Price,
                    Title = importBook.Title
                };
                _dbContext.Books.Add(book);
                _dbContext.SaveChanges();
                keyValue.Add(importBook.Id, book.BookId);
            }
            _dbContext.SaveChanges();
            return keyValue;
        }

        private void LoadSalesAndReviews(Dictionary<int, int> bookKeyMap, Dictionary<int, string> memberKeyMap)
        {
            var jsonFile = System.IO.File.ReadAllText(@"C:\Users\Roel\Documents\MCSD\PROJECT\YarnsAndMobileRCOnlineBookStore\Yarns_SaleReview.json");
            var importSaleReviews = JsonConvert.DeserializeObject<List<SaleReviewImport>>(jsonFile);

            foreach (var importSaleReview in importSaleReviews)
            {
                var book = _dbContext.Books.Find(bookKeyMap[importSaleReview.BookId]);
                var member = _dbContext.Members.Find(memberKeyMap[importSaleReview.MemberId]);

                if (importSaleReview.SaleDate.HasValue)
                {
                    var sale = new Sale
                    {
                        PurchaseDate = importSaleReview.SaleDate.GetValueOrDefault(),
                        Price = importSaleReview.SalePrice.GetValueOrDefault(),
                        Title = book.Title,
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
                        Title = importSaleReview.ReviewTitle,
                        Text = importSaleReview.ReviewBody,
                        StarRating = importSaleReview.ReviewRating,
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
