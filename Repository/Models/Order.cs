using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class Order
    {
        public Order()
        {
            PartsInOrders = new HashSet<PartsInOrder>();
        }

        public int OrderNumber { get; set; }
        public int? AccountNumber { get; set; }
        public int? StoreNumber { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? DateTime { get; set; }

        public virtual Account AccountNumberNavigation { get; set; }
        public virtual Store StoreNumberNavigation { get; set; }
        public virtual ICollection<PartsInOrder> PartsInOrders { get; set; }
    }
}
