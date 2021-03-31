using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class Account
    {
        public Account()
        {
            Orders = new HashSet<Order>();
        }

        public int AccountNumber { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal? PhoneNumber { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public decimal? ZipCode { get; set; }
        public string StreetAddress { get; set; }
        public byte Permissions { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public int? DefaultStore { get; set; }

        public virtual Store DefaultStoreNavigation { get; set; }
        public virtual State StateNameNavigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
