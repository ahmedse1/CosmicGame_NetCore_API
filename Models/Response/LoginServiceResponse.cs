using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Models.Response
{
    public class LoginServiceResponse
    {
        public string access_token { get; set; }
        public string struserId { get; set; }
        public int userId { get; set; }
        public string userName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool isResetPassword { get; set; }
        public string displayMessage { get; set; }
        public string LoginInfoId { get; set; }
        public bool? IsAllowMultiLogin { get; set; }
        public bool? IsApproved { get; set; }
    }
    public class LoginResponse
    {
        public HttpStatusCode status { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public LoginServiceResponse result { get; set; }
    }

}