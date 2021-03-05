using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YarnsAndMobileRCOnlineBookStore.Models.Data
{
    public class Member : IdentityUser
    {
        public int MemberId { get; set; }
       
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }
     
        [MaxLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(100)]
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email Address")]
        public override string Email { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Phone> PhoneNumbers { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
