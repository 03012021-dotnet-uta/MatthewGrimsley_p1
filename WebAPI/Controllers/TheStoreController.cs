using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic;
using UniversalModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TheStoreController : ControllerBase
    {
        private readonly UserMethods _userMethods;
        private readonly StoreMethods _storeMethods;
        public TheStoreController(UserMethods userMethods, StoreMethods storeMethods)
        {
            _userMethods = userMethods;
            _storeMethods = storeMethods;
        }

        /// <summary>
        /// Attempts to sign in a user. Returns a session token if successful.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("login/{username}")]
        public ActionResult<LoginToken> LogIn(String username)
        {
            if(!ModelState.IsValid)
            {
                return StatusCode(400);
            }

            LoginData loginData = new LoginData(username);
            LoginToken loginToken = _userMethods.LogIn(loginData);

            if(loginToken == null)
            {
                return StatusCode(400);
            }
            if(loginToken.Username == "")
            {
                return StatusCode(404);
            }
            if(loginToken.Token == "")
            {
                return StatusCode(401);
            }
            StatusCode(202);
            return loginToken;
        }

        /// <summary>
        /// Signs out the user whose is identified by their token in the session cookies
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        public ActionResult LogOut()
        {
            if(!ModelState.IsValid)
            {
                return StatusCode(400);
            }

            string token = Request.Cookies["token"];
            if(_userMethods.LogOut(token))
            {
                return StatusCode(202);
            }
            else
            {
                return StatusCode(404);
            }
        }

        /// <summary>
        /// Creates a new Customer account
        /// </summary>
        /// <param name="newAccountData"></param>
        /// <returns></returns>
        [HttpPost("newaccount")]
        public ActionResult CreateAccount([FromBody] NewAccountData newAccountData)
        {
            if(!ModelState.IsValid)
            {
                return StatusCode(400);
            }

            if(_userMethods.CreateUserAccount(newAccountData))
            {
                return StatusCode(201);
            }
            else
            {
                return StatusCode(400);
            }
        }

        /// <summary>
        /// Returns a list of every State object
        /// </summary>
        /// <returns></returns>
        [HttpGet("states")]
        public ActionResult<List<State>> States()
        {
            if(!ModelState.IsValid)
            {
                return StatusCode(400);
            }

            List<State> states = _userMethods.GetStates();

            if(states == null || states.Count == 0)
            {
                return StatusCode(404);
            }
            StatusCode(200);
            return states;
        }

        /// <summary>
        /// Returns a list of every Store
        /// </summary>
        /// <returns></returns>
        [HttpGet("stores")]
        public ActionResult<List<Store>> Stores()
        {
            if(!ModelState.IsValid)
            {
                return StatusCode(400);
            }

            List<Store> stores = _userMethods.GetStores();

            if(stores == null || stores.Count == 0)
            {
                return StatusCode(404);
            }
            StatusCode(200);
            return stores;
        }

        /// <summary>
        /// Returns a list of products that includes a field for the quantity available
        /// at the specified store.
        /// </summary>
        /// <param name="storeNumber"></param>
        /// <returns></returns>
        [HttpGet("store/{storeNumber}/inventory")]
        public ActionResult<List<Product>> GetStoreInventory(int storeNumber)
        {
            if(!ModelState.IsValid)
            {
                return StatusCode(400);
            }

            List<Product> products = _userMethods.GetStoreInventory(storeNumber);

            if(products == null || products.Count == 0)
            {
                return StatusCode(404);
            }
            StatusCode(200);
            return products;
        }

        /// <summary>
        /// Sets the default store for the user who is identified by the session cookies
        /// </summary>
        /// <param name="storeNumber"></param>
        /// <returns></returns>
        [HttpPost("store/default/{storeNumber}")]
        public ActionResult SetDefaultStore(int storeNumber)
        {
            if(!ModelState.IsValid)
            {
                return StatusCode(400);
            }

            string token = Request.Cookies["token"];
            bool success = _userMethods.SetDefaultStore(token, storeNumber);

            if(!success)
            {
                return StatusCode(401);
            }
            return StatusCode(202);
        }

        /// <summary>
        /// Places an order for the user who is identified by the session cookies.
        /// Updates the store's inventory.
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        [HttpPost("placeorder")]
        public ActionResult PlaceOrder([FromBody] Cart cart)
        {
            if(!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            string token = Request.Cookies["token"];
            if(_userMethods.CreateOrder(token, cart))
            {
                return StatusCode(201);
            }
            else
            {
                return StatusCode(403);
            }
        }
    }
}
