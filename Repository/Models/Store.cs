using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class Store
    {
        public Store()
        {
            Accounts = new HashSet<Account>();
            Inventories = new HashSet<Inventory>();
            Orders = new HashSet<Order>();
        }

        public int StoreNumber { get; set; }
        public string StoreName { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public decimal? ZipCode { get; set; }
        public string StreetAddress { get; set; }
        
        public virtual State StateNameNavigation { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}