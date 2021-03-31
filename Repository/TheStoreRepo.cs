using System;
using System.Collections.Generic;
using System.Linq;
using Repository.Models;

namespace Repository
{
    public class TheStoreRepo
    {
        private readonly TheStore_DbContext _dbContext;
        public TheStoreRepo(TheStore_DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns the account associated with a username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Account GetAccount(string username)
        {
            return _dbContext.Accounts.Where(a => a.Username == username).FirstOrDefault<Account>();
        }
        public List<State> GetStates()
        {
            return _dbContext.States.ToList();
        }
    }
}
