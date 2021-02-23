using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YarnsAndMobileRCOnlineBookStore.Data;
using YarnsAndMobileRCOnlineBookStore.Models;
using YarnsAndMobileRCOnlineBookStore.Models.Data;

namespace YarnsAndMobileRCOnlineBookStore.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Member> _userManager;
        private readonly SignInManager<Member> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(
            UserManager<Member> userManager,
            SignInManager<Member> signInManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public Member user;
        public class InputModel
        {
            [Display(Name = "Account Number")]
            public string AccountNumber { get; set; }

            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            public int PhoneId { get; set; }
            public string PhoneMemberId { get; set; }
            [Phone]
            [Display(Name = "Mobile Phone Number")]
            public string Phone1 { get; set; }
            [Phone]
            [Display(Name = "Telephone Number")]
            public string Phone2 { get; set; }
            [Phone]
            [Display(Name = "Work Phone Number")]
            public string Phone3 { get; set; }
            [Phone]
            [Display(Name = "Home Phone Number")]
            public string Phone4 { get; set; }

            public int AddressId { get; set; }
            public string AddressMemberId { get; set; }

            [Display(Name = "Apartment Unit")]
            public string Line1 { get; set; }

            [Display(Name = "Building Number")]
            public string Line2 { get; set; }

            [Display(Name = "Street")]
            public string Street { get; set; }

            [Display(Name = "City")]
            public string City { get; set; }

            [Display(Name = "State")]
            public string State { get; set; }

            [Display(Name = "Zip")]
            public string Zip { get; set; }
        }


        private void LoadAsync(Member member)
        {
            //User Detail
            user = _dbContext.Members.Find(member.Id);

            //Phone Details
            var phones = from phone in _dbContext.Phones select phone;
            phones = phones.Include(m => m.Members);
            phones = phones.Where(m => m.Members.Id.Contains(member.Id));

            //Address Details
            var addresses = from address in _dbContext.Addresses select address;
            addresses = addresses.Include(m => m.Members);
            addresses = addresses.Where(m => m.Members.Id.Contains(member.Id));

            Input = new InputModel();

            Input.UserName = user.UserName;
            Input.FirstName = user.FirstName;
            Input.LastName = user.LastName;
            Input.AccountNumber = user.AccountNumber;

            if (phones.Any())
            {
                Input.PhoneId = phones.Select(p => p.PhoneId).SingleOrDefault();
                Input.PhoneMemberId = phones.Select(p => p.Members.Id).SingleOrDefault();
                Input.Phone1 = phones.Select(p => p.Phone1).SingleOrDefault();
                Input.Phone2 = phones.Select(p => p.Phone2).SingleOrDefault();
                Input.Phone3 = phones.Select(p => p.Phone3).SingleOrDefault();
                Input.Phone4 = phones.Select(p => p.Phone4).SingleOrDefault();
            }
            else
            {
                Input.Phone1 = null;
                Input.Phone2 = null;
                Input.Phone3 = null;
                Input.Phone4 = null;
            }

            if (addresses.Any())
            {
                Input.AddressId = addresses.Select(a => a.AddressId).SingleOrDefault();
                Input.AddressMemberId = addresses.Select(a => a.Members.Id).SingleOrDefault();
                Input.Line1 = addresses.Select(a => a.Line1).SingleOrDefault();
                Input.Line2 = addresses.Select(a => a.Line2).SingleOrDefault();
                Input.Street = addresses.Select(a => a.Street).SingleOrDefault();
                Input.City = addresses.Select(a => a.City).SingleOrDefault();
                Input.State = addresses.Select(a => a.State).SingleOrDefault();
                Input.Zip = addresses.Select(a => a.Zip).SingleOrDefault();
            }
            else
            {
                Input.Line1 = null;
                Input.Line2 = null;
                Input.Street = null;
                Input.City = null;
                Input.State = null;
                Input.Zip = null;
            }
        }

        public async Task<IActionResult> OnGetAsync(Member member)
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (User.IsInRole("Admin") == true)
                {
                    user = _dbContext.Members.Find(member.Id);
                }
                else
                {

                    user = await _userManager.GetUserAsync(User);
                }
            }
            if (user == null)
            {
                return NotFound($"Unable to load user with UserId '{_userManager.GetUserId(User)}'.");
            }

            LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Member member)
        {
            //########### USER Details ###########

            if (_signInManager.IsSignedIn(User))
            {
                if (User.IsInRole("Admin") == true)
                {
                    user = _dbContext.Members.Find(member.Id);
                }
                else
                {

                    user = await _userManager.GetUserAsync(User);
                }
            }
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (ModelState.IsValid)
            {
                LoadAsync(user);
                return Page();
            }

            member = await _dbContext.Members.FindAsync(user.Id);

            if (Input.UserName == "")
            {
                StatusMessage = "Username should not be mepty.";
                return RedirectToPage();
            }
            else if (Input.UserName != member.UserName)
            {
                member.UserName = Input.UserName;
                _dbContext.Update(member);
            }

            if (Input.FirstName == "")
            {
                StatusMessage = "Firstname should not be mepty.";
                return RedirectToPage();
            }
            else if (Input.FirstName != member.FirstName)
            {
                member.FirstName = Input.FirstName;
                _dbContext.Update(member);
            }

            if (Input.LastName == "")
            {
                StatusMessage = "Lastname should not be mepty.";
                return RedirectToPage();
            }
            else if (Input.LastName != member.LastName)
            {
                member.LastName = Input.LastName;
                _dbContext.Update(member);
            }

            //########### PHONES #############
            var phones = from p in _dbContext.Phones select p;
            phones = phones.Include(m => m.Members);
            var phoneIds = from id in phones select id.Members.Id; // get the list of memberid from the phone table
            var phoneExist = (from id in phoneIds select id).Where(i => i.Contains(member.Id));
            phones = phones.Where(m => m.Members.Id.Contains(member.Id));

            var phoneId = phones.Select(p => p.PhoneId).SingleOrDefault();
            var phone1 = phones.Select(p => p.Phone1).SingleOrDefault();
            var phone2 = phones.Select(p => p.Phone2).SingleOrDefault();
            var phone3 = phones.Select(p => p.Phone3).SingleOrDefault();
            var phone4 = phones.Select(p => p.Phone4).SingleOrDefault();


            Phone phone = new Phone();

            if (phoneExist.Any())
            {
                phone.PhoneId = phoneId;
                _dbContext.Phones.Attach(phone);
                _dbContext.Entry(phone).State = EntityState.Modified;
            }

            if (phoneExist.Any())
            {
                phone.Members = member;
                _dbContext.Phones.Attach(phone);
                _dbContext.Entry(phone).State = EntityState.Modified;
            }
            else
            {
                phone.Members = member;
                _dbContext.Phones.Add(phone);
            }

            if (Input.Phone1 == "" || Input.Phone1 == null) { }
            else if (Input.Phone1 != phone1)
            {
                phone.Phone1 = Input.Phone1;
                if (phoneExist.Any())
                {
                    _dbContext.Phones.Attach(phone);
                    _dbContext.Entry(phone).State = EntityState.Modified;
                }
                else
                    _dbContext.Phones.Add(phone);
            }

            if (Input.Phone2 == "" || Input.Phone2 == null) { }
            else if (Input.Phone2 != phone2)
            {
                phone.Phone2 = Input.Phone2;
                if (phoneExist.Any())
                {
                    _dbContext.Phones.Attach(phone);
                    _dbContext.Entry(phone).State = EntityState.Modified;
                }
                else
                    _dbContext.Phones.Add(phone);
            }

            if (Input.Phone3 == "" || Input.Phone3 == null) { }
            else if (Input.Phone3 != phone3)
            {
                phone.Phone3 = Input.Phone3;
                if (phoneExist.Any())
                {
                    _dbContext.Phones.Attach(phone);
                    _dbContext.Entry(phone).State = EntityState.Modified;
                }
                else
                    _dbContext.Phones.Add(phone);
            }

            if (Input.Phone4 == "" || Input.Phone4 == null) { }
            else if (Input.Phone4 != phone4)
            {
                phone.Phone4 = Input.Phone4;
                if (phoneExist.Any())
                {
                    _dbContext.Phones.Attach(phone);
                    _dbContext.Entry(phone).State = EntityState.Modified;
                }
                else
                    _dbContext.Phones.Add(phone);
            }

            //########### ADDRESS ##############
            var addresses = from add in _dbContext.Addresses select add;
            addresses = addresses.Include(m => m.Members);
            var addressIds = from a in addresses select a.Members.Id; // get the list of memberid from the address table
            var addressExist = (from ad in addressIds select ad).Where(a => a.Contains(member.Id));
            addresses = addresses.Where(m => m.Members.Id.Contains(member.Id));

            var addressId = addresses.Select(a => a.AddressId).SingleOrDefault();

            var line1 = addresses.Select(a => a.Line1).SingleOrDefault();
            var line2 = addresses.Select(a => a.Line2).SingleOrDefault();
            var street = addresses.Select(a => a.Street).SingleOrDefault();
            var city = addresses.Select(a => a.City).SingleOrDefault();
            var state = addresses.Select(a => a.Zip).SingleOrDefault();
            var zip = addresses.Select(a => a.Line1).SingleOrDefault();

            Address address = new Address();

            if (addressExist.Any())
            {
                address.AddressId = addressId;
                _dbContext.Addresses.Attach(address);
                _dbContext.Entry(address).State = EntityState.Modified;
            }


            if (addressExist.Any())
            {
                address.Members = member;
                _dbContext.Addresses.Attach(address);
                _dbContext.Entry(address).State = EntityState.Modified;
            }
            else
            {
                address.Members = member;
                _dbContext.Addresses.Add(address);
            }

            if (Input.Line1 == "" || Input.Line1 == null) { }
            else if (Input.Line1 != line1)
            {
                address.Line1 = Input.Line1;
                if (addressExist.Any())
                {
                    _dbContext.Addresses.Attach(address);
                    _dbContext.Entry(address).State = EntityState.Modified;
                }
                else
                    _dbContext.Addresses.Add(address);
            }

            if (Input.Line2 == "" || Input.Line2 == null) { }
            else if (Input.Line2 != line2)
            {
                address.Line2 = Input.Line2;
                if (addressExist.Any())
                {
                    _dbContext.Addresses.Attach(address);
                    _dbContext.Entry(address).State = EntityState.Modified;
                }
                else
                    _dbContext.Addresses.Add(address);
            }

            if (Input.Street == "" || Input.Street == null) { }
            else if (Input.Street != street)
            {
                address.Street = Input.Street;
                if (addressExist.Any())
                {
                    _dbContext.Addresses.Attach(address);
                    _dbContext.Entry(address).State = EntityState.Modified;
                }
                else
                    _dbContext.Addresses.Add(address);
            }

            if (Input.City == "" || Input.City == null) { }
            else if (Input.City != city)
            {
                address.City = Input.City;
                if (addressExist.Any())
                {
                    _dbContext.Addresses.Attach(address);
                    _dbContext.Entry(address).State = EntityState.Modified;
                }
                else
                    _dbContext.Addresses.Add(address);
            }

            if (Input.State == "" || Input.State == null) { }
            else if (Input.State != state)
            {
                address.State = Input.State;
                if (addressExist.Any())
                {
                    _dbContext.Addresses.Attach(address);
                    _dbContext.Entry(address).State = EntityState.Modified;
                }
                else
                    _dbContext.Addresses.Add(address);
            }

            if (Input.Zip == "" || Input.Zip == null) { }
            else if (Input.Zip != zip)
            {
                address.Zip = Input.Zip;
                if (addressExist.Any())
                {
                    _dbContext.Addresses.Attach(address);
                    _dbContext.Entry(address).State = EntityState.Modified;
                }
                else
                    _dbContext.Addresses.Add(address);
            }


            //########### Update Database #############
            await _dbContext.SaveChangesAsync();
            if (User.IsInRole("Admin") == false)
                await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return Page();
        }
    }
}
