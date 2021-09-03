using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Request
{
    public class PlanetRequest
    {
        public int Num_Index { get; set; }

        public string Item_ID { get; set; }

        public int KID { get; set; }

        public string Item_Name { get; set; }

        public double Location_DegDig { get; set; }

        public string DMS { get; set; }

        public bool Reversing { get; set; }

        public bool NoReversing { get; set; }

        public DateTime ReversingDate { get; set; }
    }
}
