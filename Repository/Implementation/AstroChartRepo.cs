using Dapper;
using Models.Request;
using Models.Response;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class AstroChartRepo : IAstroChartRepo
    {
        private readonly IDbContext _DbContext;
        private readonly ICommonRepo _CommonRepo;

        public AstroChartRepo(IDbContext DbContext, ICommonRepo CommonRepo)
        {
            _DbContext = DbContext;
            _CommonRepo = CommonRepo;
        }
        public async Task<int> InsertBhavaAndPlanet(tblBhavaAndPlanetRequest objBhavaAndPlanet)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@wrChartHolderID", objBhavaAndPlanet.wrChartHolderID);
                param.Add("@wrBhavalst", objBhavaAndPlanet.wrBhavalst);
                param.Add("@wrPlanetlst", objBhavaAndPlanet.wrPlanetlst);
                param.Add("@wrCreatedDate", objBhavaAndPlanet.wrCreatedDate);
                var Response = await _DbContext.ExecutableData("Panel_Client_AddBhavAndPlanet", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> AstroChartRepo -> InsertBhavaAndPlanet : " + ex.Message + "::" + ex.StackTrace);
                return 0;
            }


        }

        public async Task<tblBhavaAndPlanetResponse> GetBhavaAndPlanet(int iChartHolderID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@wrChartHolderID", iChartHolderID);               
                var Response = await _DbContext.GetDataFirstDefault<tblBhavaAndPlanetResponse>("Panel_Client_LoadBhavaAndPlanet", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> AstroChartRepo -> GetBhavaAndPlanet : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }
        }
        public async Task<IEnumerable<tblVimsoChartResponse>> getVimsoChart(VimsoChartRequest obj)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ChartHolderId", obj.WrChartHolderID);
                param.Add("@MoonDegreeValue", obj.WrMoonDegree);
                var Response = await _DbContext.GetDataList<tblVimsoChartResponse>("panel_GetVimsoChartData", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> AstroChartRepo -> getVimsoChart : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<tblVimsoChartResponse>> getVimsoChart_DI_Grp(VimsoChartRequest obj)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ChartHolderId", obj.WrChartHolderID);
                param.Add("@MoonDegreeValue", obj.WrMoonDegree);
                var Response = await _DbContext.GetDataList<tblVimsoChartResponse>("panel_GetVimsoChartData_DI_GRP", param, CommandType.StoredProcedure);
                return Response;
            }
            catch (Exception ex)
            {
                await _CommonRepo.setErrorData("Api.Repository -> AstroChartRepo -> getVimsoChart : " + ex.Message + "::" + ex.StackTrace);
                return null;
            }
        }
    }
}
