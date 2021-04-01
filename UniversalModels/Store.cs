namespace UniversalModels
{
    public class Store
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public decimal? ZipCode { get; set; }
        public string StreetAddress { get; set; }
        
        public Store(int store_number, string name, string city, string stateName, decimal zipCode, string streetAddress)
        {
            Number = store_number;
            Name = name;
            City = city;
            StateName = stateName;
            ZipCode = zipCode;
            StreetAddress = streetAddress;
        }
    }
}