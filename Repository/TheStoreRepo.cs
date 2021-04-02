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

        /// <summary>
        /// Returns a list of all States in the database
        /// </summary>
        /// <returns></returns>
        public List<State> GetStates()
        {
            return _dbContext.States.ToList();
        }

        /// <summary>
        /// Returns the first State whose name matches the parameter
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        public State GetState(string stateName)
        {
            return _dbContext.States.Where(s => s.StateName == stateName).FirstOrDefault<State>();
        }

        /// <summary>
        /// Adds an account to the database
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool AddAccount(Account account)
        {
            _dbContext.Accounts.Add(account);
            _dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Returns a list of all Stores in the database
        /// </summary>
        /// <returns></returns>
        public List<Store> GetStores()
        {
            return _dbContext.Stores.ToList();
        }

        /// <summary>
        /// Returns a list of all Parts in the database
        /// </summary>
        /// <returns></returns>
        public List<Part> GetProducts()
        {
            return _dbContext.Parts.ToList();
        }

        /// <summary>
        /// Sets the specified user's default store to the specifed store
        /// </summary>
        /// <param name="username"></param>
        /// <param name="storeNumber"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a list of all Inventory entries in the database for the specified store
        /// </summary>
        /// <param name="storeNumber"></param>
        /// <returns></returns>
        public List<Inventory> GetStoreInventory(int storeNumber)
        {
            return _dbContext.Inventories.Where(i => i.StoreNumber == storeNumber).ToList<Inventory>();
        }

        /// <summary>
        /// Decreases the inventory for the specified part at the specified store by the
        /// specified amount. Returns true if successful.
        /// </summary>
        /// <param name="storeNumber"></param>
        /// <param name="partNumber"></param>
        /// <param name="quantitySold"></param>
        /// <returns></returns>
        public bool ReduceInventory(int storeNumber, int partNumber, int quantitySold)
        {
            Inventory partInventory =_dbContext.Inventories.
                Where(i => i.StoreNumber == storeNumber).
                Where(i => i.PartNumber == partNumber).FirstOrDefault<Inventory>();

            int quantity = partInventory.Quantity ?? default(int);
            quantity -= quantitySold;
            if(quantity < 0)
            {
                return false;
            }
            else
            {
                partInventory.Quantity = quantity;
                _dbContext.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Adds an order to the database. Returns the new order number.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int AddOrder(Order order)
        {
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
            return _dbContext.Orders.Max(x => x.OrderNumber);
        }

        /// <summary>
        /// Adds a part to the list of parts in an order
        /// </summary>
        /// <param name="part"></param>
        public void AddPartToOrder(PartsInOrder part)
        {
            _dbContext.PartsInOrders.Add(part);
            _dbContext.SaveChanges();
        }
    }
}
