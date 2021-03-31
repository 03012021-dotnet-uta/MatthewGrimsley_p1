﻿using System;
using System.Collections.Generic;
using UniversalModels;
using Repository;
using System.Linq;

namespace BusinessLogic
{
    public static class Credentialer
    {
        private static Dictionary<string /* Token */, string /* Username */> _activeUsers = new Dictionary<string, string>();

        /// <summary>
        /// Returns the username associated with a session token. Returns an empty string if token is not found.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetUsernameFromToken(string token)
        {
            if(_activeUsers.ContainsKey(token))
            {
                return _activeUsers[token];
            }
            return "";
        }

        /// <summary>
        /// Adds the user to the list of active users.
        /// Returns a token used for permissions verification.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string LogInUser(string username)
        {
            // If the username is already in the _activeUsers, delete it
            foreach (var item in _activeUsers.Where(e => e.Value == username))
            {
                _activeUsers.Remove(item.Key);
            }

            string token = Guid.NewGuid().ToString();
            _activeUsers.Add(token, username);
            return token;
        }
    }

}