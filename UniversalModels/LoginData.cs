using System;

namespace UniversalModels
{
    /// <summary>
    /// Contains the information required to LogIn a user, a username and a hash.
    /// </summary>
    public class LoginData
    {
        public string username { get; set; }
        public string hash { get; set; }

        public LoginData(string username, string hash = "")
        {
            this.username = username;
            this.hash = hash;
        }
    }
}
