using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YarnsAndMobileRCOnlineBookStore.Models.Data;

namespace YarnsAndMobileRCOnlineBookStore.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int StarRating { get; set; }
        public virtual Book Books { get; set; }
        public virtual Member Members{ get; set; }
        public DateTime? ReviewDate { get; set; }

    }
}
