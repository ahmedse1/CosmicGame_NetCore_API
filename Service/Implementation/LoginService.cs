using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Request;
using Models.Response;
using Repository.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Services.Implementation
{
    public class LoginService : ILoginService
    {
        private readonly IHostingEnvironment environment;
        private ILoginRepo _LoginRepo;
        private readonly IConfiguration _configuration;
        public LoginService(ILoginRepo loginRepo, IConfiguration configuration, IHostingEnvironment _environment)
        {
            _LoginRepo = loginRepo;
            _configuration = configuration;
            environment = _environment;
        }
        public async Task<LoginServiceResponse> AuthenticateUser(LoginRequest userDetails)
        {
            LoginServiceResponse objLoginResponse = new LoginServiceResponse();
            objLoginResponse.userId = -1;
            objLoginResponse.userName = "";
            objLoginResponse.isResetPassword = false;
            objLoginResponse.displayMessage = "";
            objLoginResponse.LoginInfoId = "";
            try
            {
                userDetails.Password = PasswordHasher.Encrypt(userDetails.Password, Constants.Privatekey);
                var users = await _LoginRepo.AuthenticateUser(userDetails);
                if (users.Count() == 0)
                {
                    objLoginResponse.displayMessage = "Invalid username or password";
                    return objLoginResponse;
                }
                else if (users.Count() > 1)
                {
                    objLoginResponse.displayMessage = "Multiple User found";
                    return objLoginResponse;
                }
                var user = users.FirstOrDefault();
                if (user != null)
                {
                    if (user.IsApproved != true)
                    {
                        string strToken = PasswordHasher.Encrypt(user.userId.ToString() + ";" + clsCommon.GetDateTime(), Constants.Privatekey);
                        strToken = System.Net.WebUtility.UrlEncode(strToken);
                        string Strbody = "";
                        string strSubject = "Email Verification - Cosmic Game";
                        Strbody = clsCommon.GetTemplateBody(environment.WebRootPath + "/EmailTemplates/RegistrationVerification.html");
                        Strbody = Strbody.Replace("`name`", user.Email);
                        Strbody = Strbody.Replace("`mobileno`", user.Mobile);
                        Strbody = Strbody.Replace("`email`", user.Email);
                        Strbody = Strbody.Replace("`Link`", userDetails.URL + "verify?t=" + strToken);
                        clsCommon.SendEmail(user.Email, "", strSubject, Strbody);

                        objLoginResponse.displayMessage = "Your account is not Approved, please verify Mail ID.";
                        return objLoginResponse;
                    }

                    //AppisDemoUser check

                    //user.LoginInfoId = PasswordHasher.Encrypt(user.LoginInfoId, Constants.Privatekey);
                    objLoginResponse.userId = user.userId;
                    objLoginResponse.userName = user.userName;
                    objLoginResponse.displayMessage = "";
                    objLoginResponse.LoginInfoId = user.LoginInfoId;
                    return objLoginResponse;
                }
                else
                {
                    objLoginResponse.displayMessage = "Invalid Username or Password.";
                    return objLoginResponse;
                }
            }
            catch (Exception ex)
            {
                objLoginResponse.displayMessage = "something went wrong." + ex.Message;
                return objLoginResponse;
            }
        }

        public async Task<ServiceResponse> ForgotPassword(string strEmail)
        {
            ServiceResponse objResponse = new ServiceResponse();
            var objUser = await _LoginRepo.IsEmailExist(strEmail);
            if (objUser != null && objUser.WrEmail != "")
            {
                string Strbody = "";
                string strSubject = "Reset Password - Cosmic Game";
                Strbody = clsCommon.GetTemplateBody(environment.WebRootPath + "/EmailTemplates/Forgotpassword.html");
                Strbody = Strbody.Replace("`name`", objUser.WrUserName);
                //Strbody = Strbody.Replace("`Link`", "");
                //Strbody = Strbody.Replace("`Message`", "Click Below link to reset your password.");
                Strbody = Strbody.Replace("`Message`", "Use below login credential to access cosmic game.");
                Strbody = Strbody.Replace("`Email`", objUser.WrEmail);
                Strbody = Strbody.Replace("`Password`", PasswordHasher.Decrypt(objUser.WrPassword, Constants.Privatekey));
                clsCommon.SendEmail(objUser.WrEmail, "", strSubject, Strbody);

                objResponse.success = true;
                objResponse.message = "Reset password successfully.<br>Please check Email.";
                objResponse.status = HttpStatusCode.OK;
            }
            else
            {
                objResponse.success = false;
                objResponse.message = "Email does not exist.<br>please try with registered Email!";
                objResponse.status = HttpStatusCode.Conflict;
            }
            return objResponse;
        }


        public async Task<(string, string)> CreateJwtToken(int UserID, string UserName, string Ip, string Browser)
        {
            var Response = await _LoginRepo.AddTokenInDb(UserID, DateTime.UtcNow.AddHours(5.5).ToString(GlobalVars.appDateFormatFull), Ip, Browser);
            string strHash = Response.Item1;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(GlobalVars.JwtKey);
            double expMinutesConfig = Convert.ToDouble(_configuration["BearerTokens:AccessTokenExpirationMinutes"].ToString());
            string vissuer = _configuration["AuthToken:Issuer"].ToString();
            string vaudience = _configuration["AuthToken:Audience"].ToString();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier,Utility.PasswordHasher.Encrypt(UserID.ToString(),Constants.Privatekey)),
                    new Claim(ClaimTypes.Name,UserName),
                    new Claim(ClaimTypes.Hash,strHash),
                }),
                Expires = DateTime.UtcNow.AddMinutes(expMinutesConfig),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = vissuer,
                Audience = vaudience,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string strUserLoginInfoID = PasswordHasher.Encrypt(Convert.ToString(Response.Item2), Constants.Privatekey);
            return (tokenHandler.WriteToken(token), strUserLoginInfoID);
        }

        public async Task<ServiceResponse> RegisterUser(RegisterRequest userDetails)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                userDetails.Password = PasswordHasher.Encrypt(userDetails.Password, Constants.Privatekey);
                var Response = await _LoginRepo.RegisterUser(userDetails);
                if (Response.isExist || Response.userId > 0)
                {
                    if (Response.isExist)
                    {
                        objResponse.success = !Response.isExist;
                        objResponse.message = "Email already exist.<br>please try with different one!";
                        objResponse.status = HttpStatusCode.Conflict;
                    }
                    else
                    {
                        string strToken = PasswordHasher.Encrypt(Response.userId.ToString() + ";" + clsCommon.GetDateTime(), Constants.Privatekey);
                        strToken = System.Net.WebUtility.UrlEncode(strToken);
                        string Strbody = "";
                        string strSubject = "Email Verification - Cosmic Game";
                        Strbody = clsCommon.GetTemplateBody(environment.WebRootPath + "/EmailTemplates/RegistrationVerification.html");
                        Strbody = Strbody.Replace("`name`", userDetails.Email);
                        Strbody = Strbody.Replace("`mobileno`", userDetails.Contact);
                        Strbody = Strbody.Replace("`email`", userDetails.Email);
                        Strbody = Strbody.Replace("`Link`", userDetails.URL + "verify?t=" + strToken);
                        clsCommon.SendEmail(userDetails.Email, "", strSubject, Strbody);

                        objResponse.success = true;
                        objResponse.result = Response.userId;
                        objResponse.message = "Register successfully.<br>Please check and verify email";
                        objResponse.status = HttpStatusCode.OK;
                    }
                }
                else
                {
                    objResponse.success = false;
                    objResponse.message = "Registration Failed.<br>Please try after some time!";
                    objResponse.status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                objResponse.success = false;
                objResponse.message = "something went wrong." + ex.Message;
            }
            return objResponse;
        }
        public async Task<ServiceResponse> Userverify(string userId)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                var Response = await _LoginRepo.Userverify(userId);
                objResponse.success = Response;
            }
            catch (Exception ex)
            {
                objResponse.success = false;
                objResponse.message = "something went wrong." + ex.Message;
            }
            return objResponse;
        }


    }
}
