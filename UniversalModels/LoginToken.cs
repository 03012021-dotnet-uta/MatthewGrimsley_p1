using System;

namespace UniversalModels
{
    /// <summary>
    /// The information about the user that is required by the web site:
    /// username, token, and permissions.
    /// </summary>
    public class LoginToken
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public int Permissions { get; set; }
        public int DefaultStore { get; set; }

        public LoginToken(string username, string token, int permissions, int defaultStore)
        {
            Username = username;
            Token = token;
            Permissions = permissions;
            DefaultStore = defaultStore;
        }
    }
}
