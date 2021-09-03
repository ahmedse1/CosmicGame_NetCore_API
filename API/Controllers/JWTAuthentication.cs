using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Utility;

namespace API.Controllers
{
    public class JWTAuthentication : IAsyncActionFilter
    {
        public int UserId { get; private set; }
        public string strHash { get; private set; }
        public int LoginType { get; private set; }
        public string path { get; private set; }

        private readonly ICommonService _ICommonService;
        public JWTAuthentication(IHttpContextAccessor httpContextAccessor, ICommonService ICommonService)
        {
            _ICommonService = ICommonService;
            var claimsIdentity = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity.Claims.Count() > 0)
            {
                var nameIdentifier = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
                var userLoginInfoToken = claimsIdentity?.FindFirst(ClaimTypes.Hash);
                strHash = userLoginInfoToken?.Value;
                var userId = Utility.PasswordHasher.Decrypt(nameIdentifier?.Value, Constants.Privatekey);
                UserId = string.IsNullOrWhiteSpace(userId) ? 0 : int.Parse(userId);
                var userNameIdentifier = claimsIdentity?.FindFirst(ClaimTypes.Name);
            }
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimsIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity.Claims.Count() > 0)
            {
                //#region Check user login info token is valid
                //bool isValid = await _ICommonService.isValidUser(UserId, LoginType, strHash);
                //if (!isValid)
                //{
                //    context.Result = new UnauthorizedResult();
                //    return;
                //}
                //#endregion

                await next();
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
