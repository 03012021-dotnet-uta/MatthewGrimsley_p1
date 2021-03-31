using System;

namespace UniversalModels
{
    /// <summary>
    /// Contains all of the information required to create a new account.
    /// </summary>
    public class NewAccountData
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string StreetAddress { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
    }
}
