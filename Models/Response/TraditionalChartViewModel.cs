using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Response
{
    public class TraditionalChartViewModel
    {
        private static string[] ZodiacSigns = new string[12]{
            "Aries-மேஷம்",
            "Tauras / இடபம்",
            "Gemini/மிதுனம்",
            "Cancer கடகம்",
            "Leo / சிங்கம்",
            "Virgo / கன்னி",
            "Libra / துலாம்",
            "Scorpi/விருச்சி",
            "Sagit. / தனுசு",
            "Capric/மகரmmmம்",
            "Aquar./ கும்பம்",
            "Pisces / மீனம்"
        };
        public List<TraditionalChartCell> Cells { get; set; }
        public TraditionalChartViewModel()
        {
            Cells = new List<TraditionalChartCell>();
            for (int i = 1; i <= 12; i++)
            {
                Cells.Add(new TraditionalChartCell()
                {
                    Code = GenerateHintForCell(i)
                });
            }
        }

        private string GenerateHintForCell(int kid)
        {
            var minDegree = (kid - 1) * 30;
            var maxDegree = kid * 30;
            var zodiacSign = ZodiacSigns[kid - 1];
            return string.Format("<span class=\"fs8\" data-toggle=\"tooltip\" title=\"[{0}] {1}-{2} {3}\">", kid, minDegree, maxDegree, zodiacSign);
        }

    }
    public class TraditionalChartCell
    {
        public string Code { get; set; }
    }
}
