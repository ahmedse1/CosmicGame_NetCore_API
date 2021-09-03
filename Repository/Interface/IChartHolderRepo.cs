using Models.Request;
using Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IChartHolderRepo
    {
        Task<(int, bool)> AddEditChartHolderRecord(ChartHolderRequest obj);
        Task<IEnumerable<ChartHolderResponse>> LoadAllChartHolder(int iUserID);
        Task<ChartHolderResponse> LoadSingleChartHolder(int iUserID, int iChartHolderID);
        Task<int> DeleteChartHolderRecord(int iChartHolderID);
        Task<object> LoadCountries();
        Task<object> LoadTimeZone(string strCountryName);
        Task<IEnumerable<CurrentAddressResponse>> LoadAllCurrentAddress(int iUserID);
        Task<(int, bool)> AddEditCurrentAddress(CurrentAddressRequest obj);
        Task<CurrentAddressResponse> LoadSingleCurrentAddress(int iUserID, int iCurrentAddressID);
    }
}
