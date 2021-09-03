using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Request
{
    public class tblBhavaAndPlanetRequest
    {

        public string wrBhavalst { get; set; }
        public string wrPlanetlst { get; set; }
        public int wrChartHolderID { get; set; }
        public DateTime wrCreatedDate { get; set; }
    }
}
