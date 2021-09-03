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
    public class ChartHolderRepo : IChartHolderRepo
    {

        private readonly IDbContext _DbContext;
        private readonly ICommonRepo _CommonRepo;
        

        public ChartHolderRepo(IDbContext DbContext, ICommonRepo CommonRepo)
        {
            _DbContext = DbContext;
            _CommonRepo = CommonRepo;
            
        }

        public async Task<(int, bool)> AddEditChartHolderRecord(ChartHolderRequest obj)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@WrChartHolderID", Convert.ToInt32(obj.WrChartHolderID));
                param.Add("@WrUserID", obj.WrUserID);
                param.Add("@WrChildName", obj.WrChildName);
                param.Add("@WrFatherName", obj.WrFatherName);
                param.Add("@WrMotherName", obj.WrMotherName);
                param.Add("@WrGender", obj.WrGender);
                param.Add("@WrBirhPlace", obj.WrBirhPlace);
                param.Add("@WrCity", obj.WrCity);
                param.Add("@WrState", obj.WrState);
                param.Add("@WrCountry", obj.WrCountry);
                param.Add("@WrPostalCode", obj.WrPostalCode);
                param.Add("@WrDOB", obj.WrDOB);
                //  param.Add("@WrTOB", obj.WrTOB);
                param.Add("@WrTimeZone", obj.WrTimeZone);
                param.Add("@WrAyanamsa", obj.WrAyanamsa);
                param.Add("@WrHouseSystem", obj.WrHouseSystem);
                param.Add("@WrAynamsaPolicy", obj.WrAynamsaPolicy);
                param.Add("@WrLatLocator", obj.WrLatLocator);
                param.Add("@WrLngLocator", obj.WrLngLocator);
                param.Add("@WrLatitude", obj.WrLatitude);
                param.Add("@WrLongitude", obj.WrLongitude);
                // param.Add("@WrBirthTimeRectified", obj.WrBirthTimeRectified);
                 param.Add("@WrContactPhoneNumber", obj.WrContactPhoneNumber);
                param.Add("@WrEmail", obj.WrEmail);
                // param.Add("@WrPhoneNo", obj.WrPhoneNo);
                param.Add("@WrOutChartHolderID", DbType.Int32, direction: ParameterDirection.Output);

                List<SpOutput> OutputParameter = new List<SpOutput>();
                OutputParameter.Add(new SpOutput { ParameterName = "WrOutChartHolderID", ParamterType = typeof(int) });
                // OutputParameter.Add(new SpOutput { ParameterName = "isExist", ParamterType = typeof(int) });

                var Response = await _DbContext.ExecutableDataWithOutput("Panel_Client_AddChartHolder", param, OutputParameter, CommandType.StoredProcedure);
                int iChartHolderId = Convert.ToInt32(Response.Item2.Where(x => x.Item1 == "WrOutChartHolderID").Select(x => x.Item2).FirstOrDefault().ToString());
                return (iChartHolderId, false);
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> ChartHolderRepo -> AddEditChartHolderRecord : " + ex.Message + "::" + ex.StackTrace);
                return (0, false);
            }
        }

      



        public async Task<IEnumerable<ChartHolderResponse>> LoadAllChartHolder(int iUserID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@WrUserID", iUserID);
                var Response = await _DbContext.GetDataList<ChartHolderResponse>("Panel_Client_LoadAllChartHolder", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> ChartHolderRepo -> LoadAllChartHolder : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }
        }

        public async Task<ChartHolderResponse> LoadSingleChartHolder(int iUserID, int iChartHolderID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@WrUserID", iUserID);
                param.Add("@WrChartHolderID", iChartHolderID);
                var Response = await _DbContext.GetDataFirstDefault<ChartHolderResponse>("Panel_Client_LoadChartHolderByID", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> ChartHolderRepo -> LoadSingleChartHolder : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }
        }
        public async Task<int> DeleteChartHolderRecord(int iChartHolderID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@WrChartHolderID", iChartHolderID);
                var Response = await _DbContext.GetDataFirstDefault<int>("DeleteChartHolderByID", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> ChartHolderRepo -> DeleteChartHolderRecord : " + ex.Message + "::" + ex.StackTrace);
                return 0;
            }
        }

        public async Task<object> LoadCountries()
        {
            try
            {
                var Response = await _DbContext.GetDataList<object>("Panel_Client_LoadCountry", null, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> ChartHolderRepo -> LoadCountries : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }
        }

        public async Task<object> LoadTimeZone(string strCountryName)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@CountryName", strCountryName);
                var Response = await _DbContext.GetDataList<object>("Panel_Client_LoadTimeZome", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> ChartHolderRepo -> LoadTimeZone : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }


        }

        public async Task<IEnumerable<CurrentAddressResponse>> LoadAllCurrentAddress(int iUserID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@WrUserID", iUserID);
                var Response = await _DbContext.GetDataList<CurrentAddressResponse>("Panel_Client_LoadAllCurrentAddress", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> ChartHolderRepo -> LoadAllCurrentAddress : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }
        }


        public async Task<(int, bool)> AddEditCurrentAddress(CurrentAddressRequest obj)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@wrCurrentAddressID", obj.wrCurrentAddressID);
                param.Add("@wrUserId", obj.wrChartHolderId);
                param.Add("@wrAddress", obj.wrAddress);
                param.Add("@wrCity", obj.wrCity);
                param.Add("@wrState", obj.wrState);
                param.Add("@wrCountry", obj.wrCountry);
                param.Add("@wrPostalCode", obj.wrPostalCode);
                param.Add("@wrPhoneNumber", obj.wrPhoneNumber);
                param.Add("@wrIsActiveAddress", obj.wrIsActiveAddress);
                param.Add("@wrTimezone", obj.wrTimezone);
                param.Add("@WrLatLocator", obj.wrLatLocator);
                param.Add("@WrLngLocator", obj.wrLngLocator);
                param.Add("@WrLatitude", obj.wrLatitude);
                param.Add("@WrLongitude", obj.wrLongitude); 
                param.Add("@wrOutCurrentAddressID", DbType.Int32, direction: ParameterDirection.Output);

                List<SpOutput> OutputParameter = new List<SpOutput>();
                OutputParameter.Add(new SpOutput { ParameterName = "wrOutCurrentAddressID", ParamterType = typeof(int) });
                // OutputParameter.Add(new SpOutput { ParameterName = "isExist", ParamterType = typeof(int) });

                var Response = await _DbContext.ExecutableDataWithOutput("Panel_Client_AddCurrentAddress", param, OutputParameter, CommandType.StoredProcedure);
                int iChartHolderId = Convert.ToInt32(Response.Item2.Where(x => x.Item1 == "wrOutCurrentAddressID").Select(x => x.Item2).FirstOrDefault().ToString());
                //bool isExists = Convert.ToBoolean(Response.Item2.Where(x => x.Item1 == "isExist").Select(x => x.Item2).FirstOrDefault());
                //return (iChartHolderId, isExists);
                return (iChartHolderId, false);
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> ChartHolderRepo -> AddEditCurrentAddress : " + ex.Message + "::" + ex.StackTrace);
                return (0, false);
            }
        }

        public async Task<CurrentAddressResponse> LoadSingleCurrentAddress(int iUserID, int iCurrentAddressID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@WrUserID", iUserID);
                param.Add("@wrCurrentAddressID", iCurrentAddressID);
                var Response = await _DbContext.GetDataFirstDefault<CurrentAddressResponse>("Panel_Client_LoadCurrentAddressByID", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> ChartHolderRepo -> LoadSingleCurrentAddress : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }


        }

        
    }
}
