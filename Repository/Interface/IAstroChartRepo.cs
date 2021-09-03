using Models.Request;
using Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IAstroChartRepo
    {
        Task<int> InsertBhavaAndPlanet(tblBhavaAndPlanetRequest objBhavaAndPlanet);
        Task<IEnumerable<tblVimsoChartResponse>> getVimsoChart(VimsoChartRequest objBhavaAndPlanet);
        Task<IEnumerable<tblVimsoChartResponse>> getVimsoChart_DI_Grp(VimsoChartRequest objBhavaAndPlanet);
        Task<tblBhavaAndPlanetResponse> GetBhavaAndPlanet(int iChartHolderID);
    }
}
