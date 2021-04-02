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

        public List<UniversalModels.Store> GetStores()
        {
            List<Repository.Models.Store> repoStoreList = _repo.GetStores();
            List<UniversalModels.Store> umStoreList = new List<UniversalModels.Store>();

            foreach (var repoStore in repoStoreList)
            {
                decimal zipCode = repoStore.ZipCode ?? default(decimal);
                UniversalModels.Store umState = new UniversalModels.Store(repoStore.StoreNumber,
                    repoStore.StoreName, repoStore.City, repoStore.StateName, zipCode, repoStore.StreetAddress);
                umStoreList.Add(umState);
            }
            return umStoreList;
        }

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
    }
}