namespace UniversalModels
{
    /// <summary>
    /// Contains the information required to LogIn a user, a username and a hash.
    /// </summary>
    public class CartItem
    {
        public int PartNumber { get; set; }
        public int Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal UnitPrice { get; set; }

        public CartItem()
        {

        }
    }
}