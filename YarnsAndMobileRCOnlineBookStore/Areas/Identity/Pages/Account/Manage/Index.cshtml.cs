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
        }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public Member user;

        private async Task LoadAsync(Member member)
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

            Input = new InputModel
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccountNumber = user.AccountNumber
            };
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

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Member member)
        {

            user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
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
            }

            if (Input.FirstName == "")
            {
                StatusMessage = "Firstname should not be mepty.";
                return RedirectToPage();
            }
            else if (Input.FirstName != member.FirstName)
            {
                member.FirstName = Input.FirstName;
            }

            if (Input.LastName == "")
            {
                StatusMessage = "Lastname should not be mepty.";
                return RedirectToPage();
            }
            else if (Input.LastName != member.LastName)
            {
                member.LastName = Input.LastName;
            }

            await _dbContext.SaveChangesAsync();
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
