using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class Part
    {
        public Part()
        {
            Inventories = new HashSet<Inventory>();
            PartsInOrders = new HashSet<PartsInOrder>();
        }

        public int PartNumber { get; set; }
        public string PartName { get; set; }
        public string PartDescription { get; set; }
        public decimal UnitPrice { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal? SalePercent { get; set; }
        public string ImageLink { get; set; }
        public string ImageCredit { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<PartsInOrder> PartsInOrders { get; set; }
    }
}