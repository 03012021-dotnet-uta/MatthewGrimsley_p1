using System.Collections.Generic;

namespace UniversalModels
{
    /// <summary>
    /// Contains the information required to LogIn a user, a username and a hash.
    /// </summary>
    public class Cart
    {
        public int StoreNumber { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public long DateTime { get; set; }
        public CartItem[] productList { get; set; }

        public Cart()
        {

        }
    }
}
