using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class PartsInOrder
    {
        public int OrderNumber { get; set; }
        public int PartNumber { get; set; }
        public decimal? UnitPrice { get; set; }
        public string UnitOfMeasure { get; set; }
        public int? Quantity { get; set; }

        public virtual Order OrderNumberNavigation { get; set; }
        public virtual Part PartNumberNavigation { get; set; }
    }
}