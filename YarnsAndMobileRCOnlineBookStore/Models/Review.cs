﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public int? StarRating { get; set; }
        public DateTime? ReviewDate { get; set; }

        [DisplayName("Sale Price")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal? SalePrice { get; set; }
        public DateTime? SaleDate { get; set; }
        public virtual Book Books { get; set; }
        public virtual Member Members { get; set; }
    }
}
