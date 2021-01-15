using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YarnsAndMobileRCOnlineBookStore.Models.Data;

namespace YarnsAndMobileRCOnlineBookStore.Models
{
    public class Phone
    {
        [Key]
        public int PhoneId { get; set; }
        public int AreaCode { get; set; }
        public int PhoneNumber { get; set; }
        public int Extension { get; set; }
        public virtual IEnumerable<Member> Members { get; set; }
    }
}
