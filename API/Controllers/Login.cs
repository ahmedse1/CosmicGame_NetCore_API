using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Request;
using Models.Response;
using Services.Interface;
using System;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Login : ControllerBase
    {
        private ILoginService _ILoginService;

        public Login(ILoginService LoginService)
        {
            _ILoginService = LoginService;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("UserLogin")]
        public async Task<IActionResult> UserLogin(LoginRequest userRequest)
        {
            LoginResponse objResponse = new LoginResponse();

            try
            {
                string strReqIP = "";
                strReqIP = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                if (Request.Headers.ContainsKey("X-Forwarded-For"))
                {
                    strReqIP = Request.Headers["X-Forwarded-For"];
                }
                if (Request.Headers.ContainsKey("CF-CONNECTING-IP"))
                {
                    strReqIP = Request.Headers["CF-CONNECTING-IP"].ToString();
                }
                string IpInfo = strReqIP;
                string BroserInfo = Request.Headers["User-Agent"].ToString();
                var result = await _ILoginService.AuthenticateUser(userRequest);
                if (result.userId > 0)
                {
                    var Response = await _ILoginService.CreateJwtToken(Convert.ToInt32(result.userId), result.userName, IpInfo, BroserInfo);
                    LoginServiceResponse objLoginResponse = new LoginServiceResponse();
                    objLoginResponse.userId = result.userId;
                    objLoginResponse.access_token = Response.Item1;
                    objLoginResponse.LoginInfoId = Response.Item2;
                    objLoginResponse.userName = result.userName;
                    objLoginResponse.isResetPassword = result.isResetPassword;
                    objResponse.result = objLoginResponse;
                    objResponse.success = true;
                    objResponse.status = HttpStatusCode.OK;
                    return Ok(objResponse);
                }
                else
                {
                    objResponse.message = result.displayMessage;
                    objResponse.success = false;
                    objResponse.status = HttpStatusCode.Unauthorized;
                    return Ok(objResponse);
                }
            }
            catch (Exception ex)
            {
                objResponse.message = ex.Message;
            }
            objResponse.status = HttpStatusCode.Unauthorized;
            objResponse.result = null;
            objResponse.success = false;
            return Ok(objResponse);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("UserRegister")]
        public async Task<IActionResult> UserRegister(RegisterRequest userRequest)
        {
            var result = await _ILoginService.RegisterUser(userRequest);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("Userverify")]
        public async Task<IActionResult> Userverify(string token)
        {
            string strToken = System.Net.WebUtility.UrlDecode(token);
            var result = await _ILoginService.Userverify(token);
            return Ok(result);
        }

    [AllowAnonymous]
    [HttpPost]
    [Route("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(RegisterRequest userRequest)
    {
            var result = await _ILoginService.ForgotPassword(userRequest.Email);
        return Ok(result);
    }
}
}   
