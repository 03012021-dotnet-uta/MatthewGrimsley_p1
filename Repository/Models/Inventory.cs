using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class Inventory
    {
        public int StoreNumber { get; set; }
        public int PartNumber { get; set; }
        public int? Quantity { get; set; }

        public virtual Part PartNumberNavigation { get; set; }
        public virtual Store StoreNumberNavigation { get; set; }
    }
}
