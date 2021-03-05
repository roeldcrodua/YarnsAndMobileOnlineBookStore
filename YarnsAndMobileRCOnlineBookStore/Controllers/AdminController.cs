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
using YarnsAndMobileRCOnlineBookStore.Areas.Identity.Pages.Account.Manage;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace YarnsAndMobileRCOnlineBookStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Member> _userManager;
        private readonly IWebHostEnvironment _env;

        public enum UploadType { Book, Member, SaleReview };
        public AdminController(ApplicationDbContext dbContext, UserManager<Member> userManager, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _env = environment;
        }

        //[Route("/Admin/UploadFile")]
        public IActionResult Index()
        {
            ViewData["message"] = TempData["message"];
            return View();
        }

        [BindProperty]
        public Member Users { get; set; }

        [Route("/Admin/MemberIndex")]
        public async Task<IActionResult> MemberIndex(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewBag.CurentSort = sortOrder;
            ViewBag.AccountSort = String.IsNullOrEmpty(sortOrder) ? "AccountNumber" : "";
            ViewBag.FirstNameSort = sortOrder == "FirstName" ? "LastName" : "FirstName";
            ViewBag.LastNameSort = sortOrder == "LastName" ? "FirstName" : "LastName";
            ViewBag.EmailSort = sortOrder == "Email" ? "AccountNumber" : "Email";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var members = from member in _dbContext.Members select member;

            if (!String.IsNullOrEmpty(searchString))
            {
                members = members.Where(member => member.AccountNumber.Contains(searchString)
                                       || member.FirstName.Contains(searchString)
                                       || member.LastName.Contains(searchString)
                                       || member.Email.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "AccountNumber":
                    members = members.OrderBy(s => s.AccountNumber);
                    break;
                case "FirstName":
                    members = members.OrderBy(s => s.FirstName).OrderByDescending(s => s.LastName);
                    break;
                case "LastName":
                    members = members.OrderBy(s => s.LastName).OrderByDescending(s => s.FirstName);
                    break;
                case "Email":
                    members = members.OrderBy(s => s.Email);
                    break;
                default:
                    members = members.OrderBy(s => s.FirstName);
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<Member>.CreateAsync(members.AsNoTracking(), pageNumber ?? 1, pageSize));
        }


        public IActionResult ImportData()
        {
            var userAdmin = _userManager.GetUserId(User);
            ApplicationDbInitializer.SeedUsers(_userManager, userAdmin);


            var message = LoadImportData();
            if (!string.IsNullOrWhiteSpace(message))
            {
                TempData["message"] = message;
                return RedirectToAction(nameof(Index));
            }

            TempData["message"] = "Data imported";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UploadFile()
        {
            return View();
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

                if (Path.GetExtension(file.FileName).ToLower() != ".json")
                {
                    TempData["message"] = "Only json files are supported at this time";
                    return RedirectToAction(nameof(Index));
                }

                var uploadType = Enum.Parse<UploadType>(fileDataType);
                var path = GetFileName(uploadType);

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

        private string GetFileName(UploadType uploadType)
        {
            var currentDir = _env.ContentRootPath;
            Directory.CreateDirectory(Path.Combine(currentDir, "FileUploads"));
            var fileDir = Path.Combine(currentDir, "FileUploads");
            return Path.Combine(fileDir.ToString(), $"{uploadType}.json");
        }


        private Dictionary<int, string> LoadMembers()
        {
            var jsonFile = System.IO.File.ReadAllText(GetFileName(UploadType.Member));
            var importMembers = JsonConvert.DeserializeObject<List<MemberImport>>(jsonFile);

            var keyValue = new Dictionary<int, string>();
            foreach (var importMember in importMembers)
            {
                var member = new Member
                {
                    MemberId = importMember.Id,
                    AccountNumber = importMember.AccountNumber,
                    FirstName = importMember.FirstName,
                    LastName = importMember.LastName,
                    Email = importMember.Email,
                    UserName = importMember.Email
                };

                var address = new Address
                {
                    Line1 = importMember.Line1,
                    Line2 = importMember.Line2,
                    Street = importMember.Street,
                    City = importMember.City,
                    State = importMember.State,
                    Zip = importMember.Zip,
                    Members = member

                };

                var phone = new Phone
                {
                    Phone1 = importMember.Phone1,
                    Phone2 = importMember.Phone2,
                    Phone3 = importMember.Phone3,
                    Phone4 = importMember.Phone4,
                    Members = member
                };


                _dbContext.Phones.Add(phone);
                _dbContext.Addresses.Add(address);
                _dbContext.Members.Add(member);
                keyValue.Add(importMember.Id, member.Id);

            }
            _dbContext.SaveChanges();
            return keyValue;
        }

        private string LoadImportData()
        {
            var sb = new StringBuilder();
            var bookFile = GetFileName(UploadType.Book);
            var memberFile = GetFileName(UploadType.Member);
            var saleReviewFile = GetFileName(UploadType.SaleReview);
            if (!System.IO.File.Exists(bookFile))
                sb.AppendLine("You need to upload a file containing Book data");
            if (!System.IO.File.Exists(memberFile))
                sb.AppendLine("You need to upload a file containing Member data");
            if (!System.IO.File.Exists(saleReviewFile))
                sb.AppendLine("You need to upload a file containing Sale and Review data");
            if (sb.Length > 0)
                return sb.ToString();

            try
            {
                var bookKeyMap = LoadBooks();
                var memberKeyMap = LoadMembers();
                LoadSalesAndReviews(bookKeyMap, memberKeyMap);
            }
            catch
            {
                return "Import failed. Please be sure to upload the correct files.";
            }
            return null;
        }
        private Dictionary<int, int> LoadBooks()
        {
            var jsonFile = System.IO.File.ReadAllText(GetFileName(UploadType.Book));
            var importBooks = JsonConvert.DeserializeObject<List<BookImport>>(jsonFile);

            var keyValue = new Dictionary<int, int>();
            foreach (var importBook in importBooks)
            {
                var random = new Random();
                var picture = random.Next(0, 10000);

                var book = new Book
                {
                    AuthorFirstName = importBook.AuthorFirstName,
                    AuthorLastName = importBook.AuthorLastName,
                    CopyrightYear = importBook.Year,
                    Image = $"https://picsum.photos/200?random={picture}",
                    ISBN = importBook.ISBN,
                    SalePrice = importBook.Price,
                    Title = importBook.Title,
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
            var jsonFile = System.IO.File.ReadAllText(GetFileName(UploadType.SaleReview));
            var importSaleReviews = JsonConvert.DeserializeObject<List<SaleReviewImport>>(jsonFile);

            foreach (var importSaleReview in importSaleReviews)
            {
                var book = _dbContext.Books.Find(bookKeyMap[importSaleReview.BookId]);
                var member = _dbContext.Members.Find(memberKeyMap[importSaleReview.MemberId]);

                if (importSaleReview.SaleDate.HasValue)
                {
                    var sale = new Sale
                    {
                        PurchaseDate = importSaleReview.SaleDate.GetValueOrDefault().Date,
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

    }
}