using Models.Request;
using Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ILoginService
    {
        Task<LoginServiceResponse> AuthenticateUser(LoginRequest userDetails);
        Task<(string, string)> CreateJwtToken(int UserID, string UserName, string Ip, string Browser);
        Task<ServiceResponse> RegisterUser(RegisterRequest userDetails);
        Task<ServiceResponse> Userverify(string userId);
        Task<ServiceResponse> ForgotPassword(string strEmail);
    }
}
