using Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
   public class ChartCreator
    {
        public static TraditionalChartViewModel GetTraditionalChart(List<BhavaAndPlanetResponse> lstPlanetGridData)
        {

            TraditionalChartViewModel lstTraditionalData = new TraditionalChartViewModel();

            var lstKID1o6 = lstPlanetGridData.Where(x => x.KID <= 6).OrderBy(x => x.Location_DegDig).ToList();
            var lstKID7o12 = lstPlanetGridData.Where(x => x.KID > 6).OrderByDescending(x => x.Location_DegDig).ToList();

            PopulateTraditoinalData(lstTraditionalData, lstKID1o6);
            PopulateTraditoinalData(lstTraditionalData, lstKID7o12);

            return lstTraditionalData;
        }
        private static void PopulateTraditoinalData(TraditionalChartViewModel lstTraditionalData, List<BhavaAndPlanetResponse> lstData)
        {
            int style = lstData[0].KID > 6 ? 2 : 1; //basically changes the direction of arrow on frontend (up, down)
            foreach (var x in lstData)
            {
                var decsecMin = SDK_Communicator.ConvertDegreesToDMS(x.Location_DegDig).Substring(0, 8);
                var data = " <br />";
                if (x.Item_Name.Contains("BH :"))
                {
                    x.Item_Name = x.Item_Name.Length >= 7 ? x.Item_Name.Substring(0, 7) : x.Item_Name;
                    data += "<span class=\"bhavaStyle" + style + "\">" + x.Item_Name + " :  &nbsp;" + decsecMin + "</span>";
                }
                else
                {
                    x.Item_Name = x.Item_Name.Length >= 3 ? x.Item_Name.Substring(0, 3) : x.Item_Name;
                    data += x.Item_Name + " :  &nbsp;" + decsecMin;
                }
                lstTraditionalData.Cells[x.KID - 1].Code += data;
            }
        }

    }
}
