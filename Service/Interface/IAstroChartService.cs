using Models.Request;
using Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IAstroChartService
    {
        Task<TraditionalChartViewModel> GetAstroChart(int iUserID, string ChartHolderID);
        Task<ServiceResponse> getVimsoChart(VimsoChartRequest obj);
        Task<ServiceResponse> getVimsoChart_DI_Grp(VimsoChartRequest obj);
        Task<Tuple<List<BhavaResponse>, List<PlanetResponse>>> GenerateBhavaAndPlanet(int iUserID, string ChartHolderID);
    }
}
