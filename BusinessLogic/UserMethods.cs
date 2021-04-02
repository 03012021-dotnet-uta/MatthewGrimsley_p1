using System.Collections.Generic;
using UniversalModels;
using Repository;
using Repository.Models;
using System;

namespace BusinessLogic
{
    public class UserMethods
    {
        private readonly TheStoreRepo _repo;
        public UserMethods(TheStoreRepo repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Adds the username to the list of active users.
        /// LoginToken.Username is empty if the username is not found
        /// LoginToken.Token is empty if the password-hash doesn't match
        /// </summary>
        /// <param name="loginData"></param>
        /// <returns></returns>
        public LoginToken LogIn(LoginData loginData)
        {
            Account userAccount = _repo.GetAccount(loginData.username);
            LoginToken lt;
            if(userAccount == null)
            {
                lt = new LoginToken("", "", 0, 0);
            }
            else
            {
                string token = Credentialer.LogInUser(loginData.username);
                int defaultStore = userAccount.DefaultStore ?? default(int);
                
                lt = new LoginToken(userAccount.Username, token, userAccount.Permissions, defaultStore);
            }

            return lt;
        }

        /// <summary>
        /// Remove the token from the list of active users.
        /// Returns true if successful; returns false if token was not found.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool LogOut(string token)
        {
            return Credentialer.LogOut(token);
        }

        /// <summary>
        /// Returns a list of every state
        /// </summary>
        /// <returns></returns>
        public List<UniversalModels.State> GetStates()
        {
            List<Repository.Models.State> repoStateList = _repo.GetStates();
            List<UniversalModels.State> umStateList = new List<UniversalModels.State>();

            foreach (var repoState in repoStateList)
            {
                UniversalModels.State umState = new UniversalModels.State(repoState.StateName, repoState.Abbreviation, repoState.TaxPercent);
                umStateList.Add(umState);
            }
            return umStateList;
        }

        /// <summary>
        /// Creates a user account, with Customer permissions. Returns true if successful
        /// </summary>
        /// <param name="newAccountData"></param>
        /// <returns></returns>
        public bool CreateUserAccount(NewAccountData newAccountData)
        {
            Account newAccount = new Account();
            newAccount.Username = newAccountData.Username;
            newAccount.FirstName = newAccountData.FirstName;
            newAccount.LastName = newAccountData.LastName;
            newAccount.Email = newAccountData.Email;
            newAccount.PhoneNumber = newAccountData.PhoneNumber;
            newAccount.City = newAccountData.City;
            newAccount.StateName = newAccountData.State;
            newAccount.ZipCode = newAccountData.ZipCode;
            newAccount.StreetAddress = newAccountData.StreetAddress;
            newAccount.Permissions = 1;
            newAccount.Salt = newAccountData.Salt;
            newAccount.Hash = newAccountData.Hash;
            _repo.AddAccount(newAccount);
            return true;
        }

        /// <summary>
        /// Returns a list of every store.
        /// </summary>
        /// <returns></returns>
        public List<UniversalModels.Store> GetStores()
        {
            List<Repository.Models.Store> repoStoreList = _repo.GetStores();
            List<UniversalModels.Store> umStoreList = new List<UniversalModels.Store>();
            

            foreach (var repoStore in repoStoreList)
            {
                Repository.Models.State repoState = _repo.GetState(repoStore.StateName);
                decimal taxPercent = repoState.TaxPercent;

                decimal zipCode = repoStore.ZipCode ?? default(decimal);
                UniversalModels.Store umState = new UniversalModels.Store(repoStore.StoreNumber, repoStore.StoreName,
                    repoStore.City, repoStore.StateName, zipCode, repoStore.StreetAddress, taxPercent);
                umStoreList.Add(umState);
            }
            return umStoreList;
        }

        /// <summary>
        /// Returns a list of all available products with information on each, including
        /// the available inventory at the specified store.
        /// </summary>
        /// <param name="storeNumber"></param>
        /// <returns></returns>
        public List<Product> GetStoreInventory(int storeNumber)
        {
            List<Repository.Models.Part> repoProductList = _repo.GetProducts();
            List<UniversalModels.Product> umProductList = new List<UniversalModels.Product>();

            List<Repository.Models.Inventory> storeInventory = _repo.GetStoreInventory(storeNumber);

            foreach (var repoProduct in repoProductList)
            {
                int quantity = 0;
                var inventory = storeInventory.Find(i => i.PartNumber == repoProduct.PartNumber);
                if(inventory != null)
                {
                    quantity = inventory.Quantity ?? default(int);
                }
                string description = repoProduct.PartDescription;
                if(description == null)
                {
                    description = "";
                }
                decimal salePercent = repoProduct.SalePercent ?? default(decimal);
                UniversalModels.Product umProduct = new UniversalModels.Product(repoProduct.PartNumber,
                    repoProduct.PartName, description, repoProduct.UnitPrice,
                    repoProduct.UnitOfMeasure, salePercent, repoProduct.ImageLink, repoProduct.ImageCredit, quantity);
                umProductList.Add(umProduct);
            }
            return umProductList;
        }

        /// <summary>
        /// Sets the users default store, after verifying the user credentials.
        /// Returns true if successful, false otherwise.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="storeNumber"></param>
        /// <returns></returns>
        public bool SetDefaultStore(string token, int storeNumber)
        {
            string username = Credentialer.GetUsernameFromToken(token);
            if(username == "")
            {
                Console.WriteLine("SetDefaultStore() Failure, username was not found!");
                return false;
            }
            if(_repo.SetDefaultStore(username, storeNumber))
            {
                return true;
            }
            Console.WriteLine("SetDefaultStore() Failure, invalid store number!");
            return false;
        }

        /// <summary>
        /// Creates an order and updates the store's inventory, after verifying
        /// the user credentials.
        /// Returns true if successful, false otherwise.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cart"></param>
        /// <returns></returns>
        public bool CreateOrder(string token, Cart cart)
        {
            cart.SubTotal = Math.Truncate(1000 * cart.SubTotal) / 1000;
            cart.Tax = Math.Truncate(1000 * cart.Tax) / 1000;
            cart.Total = Math.Truncate(1000 * cart.Total) / 1000;

            string username = Credentialer.GetUsernameFromToken(token);

            DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            time = time.AddMilliseconds(cart.DateTime).ToLocalTime();
            
            Order order = new Order();
            order.AccountNumber = _repo.GetAccount(username).AccountNumber;
            order.StoreNumber = cart.StoreNumber;
            order.Subtotal = cart.SubTotal;
            order.Tax = cart.Tax;
            order.TotalPrice = cart.Total;
            order.DateTime = time;
            int orderNumber = _repo.AddOrder(order);

            foreach (var cartItem in cart.productList)
            {
                if(_repo.ReduceInventory(cart.StoreNumber, cartItem.PartNumber, cartItem.Quantity))
                {
                    PartsInOrder part = new PartsInOrder();
                    part.OrderNumber = orderNumber;
                    part.PartNumber = cartItem.PartNumber;
                    part.UnitPrice = cartItem.UnitPrice;
                    part.UnitOfMeasure = cartItem.UnitOfMeasure;
                    part.Quantity = cartItem.Quantity;
                    _repo.AddPartToOrder(part);
                }
            }
            
            return true;
        }
    }
}