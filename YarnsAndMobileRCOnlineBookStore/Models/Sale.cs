using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YarnsAndMobileRCOnlineBookStore.Models.Data;

namespace YarnsAndMobileRCOnlineBookStore.Models
{
    public class Sale
    {
        [Key]
        [DisplayName("Order Id")]
        public int OrderId { get; set; }

        [DisplayName("Purchased Date")]
        public DateTime PurchaseDate { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [DisplayName("Sale Price")]
        public decimal Price { get; set; }
        public virtual Member Members { get; set; }
        public virtual Book Books { get; set; }
    }
}
