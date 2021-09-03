
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Models.Request;
using Models.Response;
using Services.Interface;

namespace API.Controllers.Client
{
    [Route("Client/[controller]")]
    [ApiController]
    public class ChartHolderController : BaseController
    {

        private readonly IChartHolderService _ICHService;
        private readonly IAstroChartService _IACService;
        public ChartHolderController(IHttpContextAccessor httpContextAccessor, IChartHolderService ICHService, IAstroChartService IACService) : base(httpContextAccessor)
        {
            _ICHService = ICHService;
            _IACService = IACService;
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("SaveChartHolder")]
        public async Task<IActionResult> SaveChartHolder([Microsoft.AspNetCore.Mvc.FromBody] ChartHolderRequest obj)
        {
            obj.WrUserID = UserId;
            var Response = await _ICHService.AddEditCharHolderRecord(obj,UserId);
            return Ok(Response);
        }


        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("ChartHolderList")]
        public async Task<IActionResult> ChartHolderList()
        {
            var Response = await _ICHService.LoadAllChartHolder(UserId);
            return Ok(Response);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("SingleChartHolder")]
        public async Task<IActionResult> SingleChartHolder([Microsoft.AspNetCore.Mvc.FromBody] object objChartHolderID)
        {
            var Response = await _ICHService.LoadSingleChartHolder(UserId, Convert.ToInt32(objChartHolderID.ToString()));
            return Ok(Response);
        }
        
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("removeChartHolderRecord")]
        public async Task<IActionResult> removeChartHolderRecord([Microsoft.AspNetCore.Mvc.FromBody] object objChartHolderID)
        {
            var Response = await _ICHService.DeleteChartHolderRecord(Convert.ToInt32(objChartHolderID.ToString()));
            return Ok(Response);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("Countries")]
        public async Task<IActionResult> Countries()
        {
            var Response = await _ICHService.LoadCountries();
            return Ok(Response);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("TimeZone")]
        public async Task<IActionResult> TimeZone([Microsoft.AspNetCore.Mvc.FromBody] object objCountryName)
        {
            var Response = await _ICHService.LoadTimeZone(objCountryName.ToString());
            return Ok(Response);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("CurrentAddressList")]
        public async Task<IActionResult> CurrentAddressList()
        {
            var Response = await _ICHService.LoadAllCurrentAddress(UserId);
            return Ok(Response);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("AddEditCurrentAddress")]
        public async Task<IActionResult> AddEditCurrentAddress([Microsoft.AspNetCore.Mvc.FromBody] CurrentAddressRequest obj)
        {
            var Response = await _ICHService.AddEditCurrentAddress(obj);
            return Ok(Response);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("getSingleCurrentAddress")]
        public async Task<IActionResult> getSingleCurrentAddress([Microsoft.AspNetCore.Mvc.FromBody] object objCurrentAddressID)
        {
            var Response = await _ICHService.LoadSingleCurrentAddress(UserId, Convert.ToInt32(objCurrentAddressID.ToString()));
            return Ok(Response);
        }


        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("getAstroChart")]
        public async Task<IActionResult> getAstroChart([Microsoft.AspNetCore.Mvc.FromBody] object strChartHolderID)
        {
            var Response = await _IACService.GetAstroChart(UserId, strChartHolderID.ToString());
            return Ok(Response);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("getVimsoChart")]
        public async Task<IActionResult> getVimsoChart([Microsoft.AspNetCore.Mvc.FromBody] object strChartHolderID)
        {
            VimsoChartRequest obj = new VimsoChartRequest();
            obj.WrChartHolderID = strChartHolderID.ToString();
            var Response = await _IACService.getVimsoChart(obj);
            return Ok(Response);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Route("getVimsoChart_DI_Grp")]
        public async Task<IActionResult> getVimsoChart_DI_Grp([Microsoft.AspNetCore.Mvc.FromQuery] int strChartHolderID)
        {
            VimsoChartRequest obj = new VimsoChartRequest();
            obj.WrChartHolderID = strChartHolderID.ToString();
            var Response = await _IACService.getVimsoChart_DI_Grp(obj);
            return Ok(Response);
        }
    }
}
