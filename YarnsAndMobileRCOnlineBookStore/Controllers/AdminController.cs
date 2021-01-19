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

namespace YarnsAndMobileRCOnlineBookStore.Controllers
{
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
            return View();
        }

        public object ImportData()
        {
            var userAdmin = _userManager.GetUserId(User);
            ApplicationDbInitializer.SeedUsers(_userManager, userAdmin);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, string fileDataType)
        {

           

            return View();
        }

        private Dictionary<int, int> ImportMember()
        {
            var jsonFile = System.IO.File.ReadAllText(@"C:\Users\Roel\Documents\MCSD\PROJECT\YarnsAndMobileRCOnlineBookStore\Yarns_Member.json");
            var importMembers = JsonConvert.DeserializeObject<List<Member>>(jsonFile);

            var keyValue = new Dictionary<int, int>();
            foreach (var importMember in importMembers)
            {
                var address = new Address
                {
                    Street = importMember.Addresses.Line1,
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
    }
}
