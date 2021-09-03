using Models.Response;
using Models.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IChartHolderService
    {
        Task<ServiceResponse> AddEditCharHolderRecord(ChartHolderRequest obj, int iUserID);
        Task<ServiceResponse> LoadAllChartHolder(int iUserID);
        Task<ServiceResponse> LoadSingleChartHolder(int iUserID, int iChartHolderID);
        Task<ServiceResponse> DeleteChartHolderRecord(int iChartHolderID);
        Task<ServiceResponse> LoadCountries();
        Task<ServiceResponse> LoadTimeZone(string strCountryName);
        Task<ServiceResponse> LoadAllCurrentAddress(int iUserID);
        Task<ServiceResponse> AddEditCurrentAddress(CurrentAddressRequest obj);
        Task<ServiceResponse> LoadSingleCurrentAddress(int iUserID, int iCurrentAddressID);        
    }
}
