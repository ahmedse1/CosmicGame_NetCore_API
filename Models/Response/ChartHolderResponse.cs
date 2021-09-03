using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Response
{
    public class ChartHolderResponse
    {

        public int WrChartHolderID { get; set; }
        public int WrUserID { get; set; }
        public string WrChildName { get; set; }
        public string WrFatherName { get; set; }
        public string WrMotherName { get; set; }
        public bool WrGender { get; set; }
        public string WrBirhPlace { get; set; }
        public string WrCity { get; set; }
        public string WrState { get; set; }
        public string WrCountry { get; set; }
        public string WrPostalCode { get; set; }
        public string WrDOB { get; set; }
        public DateTime WrDateOfBirth { get; set; }
        public DateTime? DateDOB { get; set; }
        //  public string WrTOB { get; set; }
        public string WrTimeZone { get; set; }
        public string WrAyanamsa { get; set; }
        public string WrHouseSystem { get; set; }
        public string WrAynamsaPolicy { get; set; }
        public string WrLatLocator { get; set; }
        public string WrLngLocator { get; set; }
        public string WrLatitude { get; set; }
        public string WrLongitude { get; set; }
        // public bool WrBirthTimeRectified { get; set; }
        public string WrContactPhoneNumber { get; set; }
        public string WrEmail { get; set; }
        //  public string WrPhoneNo { get; set; }
        public bool isAdd { get; set; }

       
    }
}
