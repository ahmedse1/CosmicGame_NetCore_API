using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Response
{
   public  class tblVimsoChartResponse
    {
        public int Vimso_pd { get; set; }
        public string StarLord { get; set; }
        public int Di_Gp { get; set; }
        public int Pu_Gp { get; set; }
        public int An_Gp { get; set; }
        public int So_Gp { get; set; }
        public string S1SL { get; set; }
        public string S2SL { get; set; }
        public string S3SL { get; set; }
        public string S4SL { get; set; }
        public string S4SL_ArcDist { get; set; }
        public DateTime wrCreatedDate { get; set; }
        public DateTime DOB { get; set; }
    }
}
