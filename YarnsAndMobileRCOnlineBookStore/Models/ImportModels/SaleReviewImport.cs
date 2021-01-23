using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YarnsAndMobileRCOnlineBookStore.Views.Admin
{
    public class SaleReviewImport
    {
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime? SaleDate { get; set; }
        public decimal? SalePrice { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewBody { get; set; }
        public int? ReviewRating { get; set; }
    }
}
