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
                lt = new LoginToken("", "", 0);
            }
            else
            {
                string token = Credentialer.LogInUser(loginData.username);
                lt = new LoginToken(userAccount.Username, token, userAccount.Permissions);
            }

            return lt;
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
    }
}