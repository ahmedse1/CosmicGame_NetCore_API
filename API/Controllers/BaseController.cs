using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Utility;

namespace API.Controllers
{
    [ApiController]
    [ServiceFilter(typeof(JWTAuthentication))]
    public class BaseController : Controller
    {
         public int UserId { get; private set; }
        public string Username { get; private set; }
        public int RoleId { get; private set; }
        public string strHash { get; private set; }

        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
           
            var claimsIdentity = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity.Claims.Count() > 0)
            {
                var nameIdentifier = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
                var userLoginInfoToken = claimsIdentity?.FindFirst(ClaimTypes.Hash);
                strHash = userLoginInfoToken?.Value;
                var userId = Utility.PasswordHasher.Decrypt(nameIdentifier?.Value, Constants.Privatekey);
                UserId = string.IsNullOrWhiteSpace(userId) ? 0 : int.Parse(userId);
                var userNameIdentifier = claimsIdentity?.FindFirst(ClaimTypes.Name);
                Username = userNameIdentifier?.Value;
                
            }
        }
    }
}
