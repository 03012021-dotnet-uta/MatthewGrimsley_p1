using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic;
using UniversalModels;
using Microsoft.AspNetCore.Mvc;
using Repository;

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
            return loginToken;
        }
    }
}
