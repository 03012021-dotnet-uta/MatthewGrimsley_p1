using System;
using System.Collections.Generic;

#nullable disable

namespace Repository.Models
{
    public partial class State
    {
        public State()
        {
            Accounts = new HashSet<Account>();
            Stores = new HashSet<Store>();
        }

        public string StateName { get; set; }
        public string Abbreviation { get; set; }
        public decimal TaxPercent { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
    }
}