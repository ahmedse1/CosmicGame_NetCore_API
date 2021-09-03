using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Response
{
    public class BhavaAndPlanetResponse
    {
        public int Index { get; set; }

        public int KID { get; set; }

        public string Item_ID { get; set; }

        public string Item_Name { get; set; }

        public string Relative_Order { get; set; }

        public double Location_DegDig { get; set; }

        public string Location_DMS { get; set; }
    }
}
