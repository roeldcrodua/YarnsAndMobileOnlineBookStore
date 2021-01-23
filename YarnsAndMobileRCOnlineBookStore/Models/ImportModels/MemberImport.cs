using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YarnsAndMobileRCOnlineBookStore.Models.ImportModels
{
    public class MemberImport
    {
        public int Id { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string PhoneType1 { get; set; }
        public string Phone2 { get; set; }
        public string PhoneType2 { get; set; }
        public string Phone3 { get; set; }
        public string PhoneType3 { get; set; }
        public string Phone4 { get; set; }
        public string PhoneType4 { get; set; }
    }
}
