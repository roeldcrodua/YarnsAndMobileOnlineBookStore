using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YarnsAndMobileRCOnlineBookStore.Models.ImportModels
{
    public class BookImport
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public int? Year { get; set; }
        public Decimal Price { get; set; }
        public int Id { get; set; }
    }
}
