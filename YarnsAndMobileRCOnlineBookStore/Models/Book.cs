using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YarnsAndMobileRCOnlineBookStore.Models.Data;

namespace YarnsAndMobileRCOnlineBookStore.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public int? CopyrightYear { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public Decimal SalePrice { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
