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

        public bool AddAccount(Account account)
        {
            _dbContext.Accounts.Add(account);
            _dbContext.SaveChanges();
            return true;
        }

        public List<Store> GetStores()
        {
            return _dbContext.Stores.ToList();
        }

        public List<Part> GetProducts()
        {
            return _dbContext.Parts.ToList();
        }

        public bool SetDefaultStore(string username, int storeNumber)
        {
            Account account = _dbContext.Accounts.Where(a => a.Username == username).FirstOrDefault<Account>();
            if(account == null || !StoreExists(storeNumber))
            {
                return false;
            }
            account.DefaultStore = storeNumber;
            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Returns true iff there exists a Store in the database with store_number = storeNumber.
        /// </summary>
        /// <param name="storeNumber"></param>
        /// <returns></returns>
        private bool StoreExists(int storeNumber)
        {
            if(_dbContext.Stores.Where(s => s.StoreNumber == storeNumber).FirstOrDefault<Store>() == null)
            {
                return false;
            }
            return true;
        }

        public List<Inventory> GetStoreInventory(int storeNumber)
        {
            return _dbContext.Inventories.Where(i => i.StoreNumber == storeNumber).ToList<Inventory>();
        }
    }
}
