using Models.Request;
using Models.Response;
using Repository.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Services.Implementation
{
    public class ChartHolderService : IChartHolderService
    {

        private readonly IChartHolderRepo _ICHRepo;
        private readonly IAstroChartService _IACService;
        public ChartHolderService(IChartHolderRepo ICHRepo , IAstroChartService IACService)
        {
            _ICHRepo = ICHRepo;
            _IACService = IACService;
           
        }

        public async Task<ServiceResponse> AddEditCharHolderRecord(ChartHolderRequest obj, int iUserID)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                if (obj.WrChartHolderID == "")
                {
                    obj.WrChartHolderID = "0";
                    obj.isAdd = true;
                }

                var Response = await _ICHRepo.AddEditChartHolderRecord(obj);
                
                if (Response.Item2 || Response.Item1 > 0)
                {
                    objResponse.result = new ChartHolderRequest();

                    if (Response.Item2)
                    {
                        objResponse.success = !Response.Item2;
                        objResponse.message = "Record already exist!";
                        objResponse.status = HttpStatusCode.Conflict;
                    }
                    else
                    {
                         var response = _IACService.GenerateBhavaAndPlanet(obj.WrUserID, Response.Item1.ToString());

                        objResponse.success = true;
                        if (obj.isAdd)
                            objResponse.message = "Added successfully";
                        else
                            objResponse.message = "Updated successfully";
                        objResponse.status = HttpStatusCode.OK;
                    }
                }
                else
                {
                    objResponse.success = false;
                    if (obj.isAdd)
                        objResponse.message = "Added Failed.!";
                    else
                        objResponse.message = "Updated Failed.!";
                    objResponse.status = HttpStatusCode.InternalServerError;
                   
                }
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }

        public async Task<ServiceResponse> LoadAllChartHolder(int iUserID)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                if (iUserID > 0)
                {
                    var obj = await _ICHRepo.LoadAllChartHolder(iUserID);
                    objResponse.result = obj;
                    objResponse.success = true;
                }
                else
                {
                    objResponse.result = null;
                    objResponse.success = false;
                    objResponse.message = "Something went wrong...!";
                }
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }

        public async Task<ServiceResponse> LoadSingleChartHolder(int iUserID, int iChartHolderID)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                if (iUserID > 0)
                {
                    var obj = await _ICHRepo.LoadSingleChartHolder(iUserID, iChartHolderID);
                    objResponse.result = obj;
                    objResponse.success = true;
                }
                else
                {
                    objResponse.result = null;
                    objResponse.success = false;
                    objResponse.message = "";
                }
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }
        
        public async Task<ServiceResponse> DeleteChartHolderRecord(int iChartHolderID)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                if (iChartHolderID > 0)
                {
                    var obj = await _ICHRepo.DeleteChartHolderRecord(iChartHolderID);
                    objResponse.result = obj;
                    objResponse.success = true;
                }
                else
                {
                    objResponse.result = null;
                    objResponse.success = false;
                    objResponse.message = "";
                }
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }

        public async Task<ServiceResponse> LoadCountries()
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                var obj = await _ICHRepo.LoadCountries();
                objResponse.result = obj;
                objResponse.success = true;
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }

        public async Task<ServiceResponse> LoadTimeZone(string strCountryName)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                var obj = await _ICHRepo.LoadTimeZone(strCountryName);
                objResponse.result = obj;
                objResponse.success = true;
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }

        public async Task<ServiceResponse> LoadAllCurrentAddress(int iUserID)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                if (iUserID > 0)
                {
                    var obj = await _ICHRepo.LoadAllCurrentAddress(iUserID);
                    objResponse.result = obj;
                    objResponse.success = true;
                }
                else
                {
                    objResponse.result = null;
                    objResponse.success = false;
                    objResponse.message = "Something went wrong...!";
                }
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }

        public async Task<ServiceResponse> AddEditCurrentAddress(CurrentAddressRequest obj)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                if (obj.wrCurrentAddressID == "")
                {
                    obj.wrCurrentAddressID = "0";
                    obj.isAdd = true;
                }

                var Response = await _ICHRepo.AddEditCurrentAddress(obj);

                if (Response.Item2 || Response.Item1 > 0)
                {
                    objResponse.result = new ChartHolderRequest();

                    if (Response.Item2)
                    {
                        objResponse.success = !Response.Item2;
                        objResponse.message = "Record already exist!";
                        objResponse.status = HttpStatusCode.Conflict;
                    }
                    else
                    {
                        objResponse.success = true;
                        if (obj.isAdd)
                            objResponse.message = "Added successfully";
                        else
                            objResponse.message = "Updated successfully";
                        objResponse.status = HttpStatusCode.OK;
                    }
                }
                else
                {
                    objResponse.success = false;
                    if (obj.isAdd)
                        objResponse.message = "Added Failed.!";
                    else
                        objResponse.message = "Updated Failed.!";
                    objResponse.status = HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }
        public async Task<ServiceResponse> LoadSingleCurrentAddress(int iUserID, int iCurrentAddressID)
        {
            ServiceResponse objResponse = new ServiceResponse();
            try
            {
                if (iUserID > 0)
                {
                    var obj = await _ICHRepo.LoadSingleCurrentAddress(iUserID, iCurrentAddressID);
                    objResponse.result = obj;
                    objResponse.success = true;
                }
                else
                {
                    objResponse.result = null;
                    objResponse.success = false;
                    objResponse.message = "";
                }
            }
            catch (Exception ex)
            {
                objResponse.message = ex.ToString();
                objResponse.success = false;
                objResponse.status = HttpStatusCode.InternalServerError;
            }
            return objResponse;
        }


    }
}
