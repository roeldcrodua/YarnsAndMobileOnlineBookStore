using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YarnsAndMobileRCOnlineBookStore.Models.Data;

namespace YarnsAndMobileRCOnlineBookStore.Data
{
    public static class ApplicationDbInitializer
    {
        public static void SeedUsers(UserManager<Member> userManager, string userId = null)
        {
            if (userManager.FindByEmailAsync("jb@yarnsandmobile.com").Result == null)
            {
                var user = new Member
                {
                    UserName = "admin@yarnsandmobile.com",
                    Email = "admin@yarnsandmobile.com",
                    AccountNumber = "ADMN00000001",
                    FirstName = "User",
                    LastName = "Administrator",
                    Id = "2fcf1f39-72ec-4879-8de2-26d3470fd8d6"
                };

                IdentityResult result = userManager.CreateAsync(user, "Passw0rd!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
