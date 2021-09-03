using Dapper;
using Models.Request;
using Models.Response;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class LoginRepo : ILoginRepo
    {
        private readonly IDbContext _DbContext;
        private readonly ICommonRepo _CommonRepo;
        public LoginRepo(IDbContext DbContext, ICommonRepo CommonRepo)
        {
            _DbContext = DbContext;
            _CommonRepo = CommonRepo;
        }
        public async Task<IEnumerable<LoginServiceResponse>> AuthenticateUser(LoginRequest userDetails)
        {
            try
            {
                var selectQuery = "Panel_Login_authenticateUsers";
                DynamicParameters param = new DynamicParameters();
                param.Add("@WrUserName", userDetails.UserName);
                param.Add("@WrPassword", userDetails.Password);
                var authenticateUserResponse = await _DbContext.GetDataList<LoginServiceResponse>(selectQuery, param, CommandType.StoredProcedure);
                return authenticateUserResponse;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Repository -> LoginRepo -> AuthenticateUser : " + ex.Message + "::" + ex.StackTrace);
                throw;
            }
        }
        public async Task<(string, int)> AddTokenInDb(int UserId, string DateTime, string Ipaddress, string Browser)
        {
            string strToken = "";
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@UserID", UserId);
                param.Add("@DateTime", DateTime);
                param.Add("@IpAddress", Ipaddress);
                param.Add("@Browser", Browser);
                param.Add("@strUniqueId", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);
                param.Add("@UserloginInfoId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                List<SpOutput> OutputParameter = new List<SpOutput>();
                OutputParameter.Add(new SpOutput { ParameterName = "strUniqueId", ParamterType = typeof(string) });
                OutputParameter.Add(new SpOutput { ParameterName = "UserloginInfoId", ParamterType = typeof(int) });

                var Response = await _DbContext.ExecutableDataWithOutput("Panel_AddUserLoginInfo", param, OutputParameter, CommandType.StoredProcedure);
                strToken = Response.Item2.Where(x => x.Item1 == "strUniqueId").Select(x => x.Item2).FirstOrDefault().ToString();
                int iUserLoginInfoId = Convert.ToInt32(Response.Item2.Where(x => x.Item1 == "UserloginInfoId").Select(x => x.Item2).FirstOrDefault().ToString());
                return (strToken, iUserLoginInfoId);
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Repository -> LoginRepo -> AddTokenInDb : " + ex.Message + "::" + ex.StackTrace);
                throw;
            }
        }
        public async Task<(int userId, bool isExist)> RegisterUser(RegisterRequest obj)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Email", obj.Email);
                param.Add("@Contact", obj.Contact);
                param.Add("@password", obj.Password);
                param.Add("@iUserId", DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@isExist", DbType.Int32, direction: ParameterDirection.Output);

                List<SpOutput> OutputParameter = new List<SpOutput>();
                OutputParameter.Add(new SpOutput { ParameterName = "iUserId", ParamterType = typeof(int) });
                OutputParameter.Add(new SpOutput { ParameterName = "isExist", ParamterType = typeof(int) });
                var Response = await _DbContext.ExecutableDataWithOutput("Panel_RegisterUser", param, OutputParameter, CommandType.StoredProcedure);
                int iUserId = Convert.ToInt32(Response.Item2.Where(x => x.Item1 == "iUserId").Select(x => x.Item2).FirstOrDefault().ToString());
                bool isExists = Convert.ToBoolean(Response.Item2.Where(x => x.Item1 == "isExist").Select(x => x.Item2).FirstOrDefault());
                return (iUserId, isExists);
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Repository -> LoginRepo -> RegisterUser : " + ex.Message + "::" + ex.StackTrace);
                throw;
            }
        }
        public async Task<bool> Userverify(string userId)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@userId", userId);

                var Response = await _DbContext.ExecutableData("Panel_VerifyUser", param, CommandType.StoredProcedure);
                if (Response > 0)
                    return true;
                else 
                    return false;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Repository -> LoginRepo -> RegisterUser : " + ex.Message + "::" + ex.StackTrace);
                throw;
            }
        }

        public async Task<tblUserResponse> IsEmailExist(string strEmail)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Email", strEmail);
                //param.Add("@ISExist", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                //var Response = await _DbContext.ExecutableData("Panel_IsEmailExist", param, CommandType.StoredProcedure);
                var Response =  await _DbContext.GetDataFirstDefault<tblUserResponse>("Panel_IsEmailExist", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Repository -> LoginRepo -> RegisterUser : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }
        }
    }
}
