using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Request
{
 public  class CurrentAddressRequest
    {
        public String wrCurrentAddressID { get; set; }
        public string wrChartHolderId { get; set; }
        public string wrAddress { get; set; }
        public string wrCity { get; set; }
        public string wrState { get; set; }
        public string wrCountry { get; set; }
        public string wrPostalCode { get; set; }
        public string wrPhoneNumber { get; set; }
        public bool wrIsActiveAddress { get; set; }
        public string wrTimezone { get; set; }
        public string wrLatitude { get; set; }
        public string wrLongitude { get; set; }
        public string wrLatLocator { get; set; }
        public string wrLngLocator { get; set; }
        public bool isAdd { get; set; }
    }
}
