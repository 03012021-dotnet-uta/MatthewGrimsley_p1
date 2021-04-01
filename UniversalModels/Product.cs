namespace UniversalModels
{
    public class Product
    {
        public int PartNumber { get; set; }
        public string PartName { get; set; }
        public string PartDescription { get; set; }
        public decimal UnitPrice { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal SalePercent { get; set; }
        public string ImageLink { get; set; }
        public string ImageCredit { get; set; }
        public int Inventory { get; set; }
        public Product(int partNumber, string partName, string partDescription,
            decimal unitPrice, string unitOfMeasure, decimal salePercent,
            string imageLink, string imageCredit, int inventory)
        {
            PartNumber = partNumber;
            PartName = partName;
            PartDescription = partDescription;
            UnitPrice = unitPrice;
            UnitOfMeasure = unitOfMeasure;
            SalePercent = salePercent;
            ImageLink = imageLink;
            ImageCredit = imageCredit;
            Inventory = inventory;
        }
    }
}