using Models.Request;
using Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ILoginRepo
    {
        Task<IEnumerable<LoginServiceResponse>> AuthenticateUser(LoginRequest userDetails);
        Task<(string, int)> AddTokenInDb(int UserId, string DateTime, string Ipaddress, string Browser);
        Task<(int userId, bool isExist)> RegisterUser(RegisterRequest obj);
        Task<bool> Userverify(string userId);
        Task<tblUserResponse> IsEmailExist(string strEmail);

    }
}
