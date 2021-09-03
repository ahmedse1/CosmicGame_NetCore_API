using Models.Request;
using Models.Response;
using Newtonsoft.Json;
using Repository.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utility;

namespace Services.Implementation
{
    public class AstroChartService : IAstroChartService
    {
        public Array arr_Bhawa;
        private int IFlag = 0;
        private int sidmode = 0;
        public double ayanamshaCalculated = 0, tempAyanamshaCalculated = 0;
        public double Ayanamsha_lahiri = 0, Ayanamsha_raman = 0, Ayanamsha_krishnamurti = 0, Ayanamsha_NewCombe = 0, Ayanamsha_999 = 0;
        public double ayan_lahiri = 0, ayan_raman = 0, ayan_krishnamurti = 0, ayan_NewCombe = 0, ayan_999 = 0;
        public string ayan_lahiriDMS = "", ayan_ramanDMS = "", ayan_krishnamurtiDMS = "", ayan_NewCombeDMS = "", ayan_999DMS = "";
        public List<BhavaResponse> BhawaPlanet_BList = new List<BhavaResponse>();
        private readonly IChartHolderRepo _ICHRepo;
        private readonly IAstroChartRepo _IACRepo;
        public AstroChartService(IChartHolderRepo ICHRepo, IAstroChartRepo IACRepo)
        {
            _ICHRepo = ICHRepo;
            _IACRepo = IACRepo;
        }
        public async Task<TraditionalChartViewModel> GetAstroChart(int iUserID, string ChartHolderID)
        {
            //var cartHolderInfo = _ICHRepo.LoadSingleChartHolder(iUserID, Convert.ToInt32(ChartHolderID)).Result;
            //ChartHolderResponse userInfo = (ChartHolderResponse)cartHolderInfo;
            //Ayanamsha_NewCombe = ayan_NewCombe;
            //SDK_Communicator.SetEphePath(AppDomain.CurrentDomain.BaseDirectory);
            //var timezone = GetTimeZone(userInfo);
            ////Converting Birthdate to UTC 
            //userInfo.DateDOB = DateTime.ParseExact(userInfo.WrDOB, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //TimeUTC timeUTC = SDK_Communicator.SwitchUsersTimeToUTC(Convert.ToDateTime(userInfo.DateDOB), timezone);
            ////Calculate Julian Day
            //double julianDay = SDK_Communicator.CalculateJulianDay(timeUTC);
            //var (SelectedAyanmsaDMS, NewCombeAdjust) = GeSelectedAyanmsaDMSandAyanNewCombe(userInfo, julianDay);
            //GlobalChartCreatorDataRequest chartData = GetChartData(julianDay, timezone, SelectedAyanmsaDMS);
            //#region Bhawa List Logic

            //  List<string> BhawaPlanet_BList = new List<string>();
            //var lstBhava = GetBhawaList(userInfo, NewCombeAdjust, julianDay);
            //#endregion

            //#region Planet List Logic
            //var lstPlanetList = GetPlanetList(userInfo, NewCombeAdjust, julianDay);
            //#endregion
            var obj = _IACRepo.GetBhavaAndPlanet(Convert.ToInt32(ChartHolderID));
            if (obj.Result == null || obj.Result.wrBhavalst == null || obj.Result.wrPlanetlst == null)
            {
                await GenerateBhavaAndPlanet(iUserID, ChartHolderID);
                obj = _IACRepo.GetBhavaAndPlanet(Convert.ToInt32(ChartHolderID));
            }
            var lstBhava = JsonConvert.DeserializeObject<List<BhavaResponse>>(obj.Result.wrBhavalst);
            var lstPlanetList = JsonConvert.DeserializeObject<List<PlanetResponse>>(obj.Result.wrPlanetlst);
            #region Bhava and Planet List With Ascending Order by degree
            var lstBhavaAndPlanet = GetBhawaAndPlanetsList(lstBhava, lstPlanetList);
            lstBhavaAndPlanet.OrderBy(x => x.Location_DegDig);

            #endregion
            //SDK_Communicator.CloseSDK();
            var response = ChartCreator.GetTraditionalChart(lstBhavaAndPlanet);
            return response;
        }

        public async Task<Tuple<List<BhavaResponse>, List<PlanetResponse>>> GenerateBhavaAndPlanet(int iUserID, string ChartHolderID)
        {
            var cartHolderInfo = _ICHRepo.LoadSingleChartHolder(iUserID, Convert.ToInt32(ChartHolderID)).Result;

            ChartHolderResponse userInfo = (ChartHolderResponse)cartHolderInfo;
            Ayanamsha_NewCombe = ayan_NewCombe;
            SDK_Communicator.SetEphePath(AppDomain.CurrentDomain.BaseDirectory);
            var timezone = GetTimeZone(userInfo);
            //Converting Birthdate to UTC 
            //userInfo.DateDOB = DateTime.ParseExact(userInfo.WrDateOfBirth, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            TimeUTC timeUTC = SDK_Communicator.SwitchUsersTimeToUTC(Convert.ToDateTime(userInfo.WrDateOfBirth), timezone);
            //Calculate Julian Day
            double julianDay = SDK_Communicator.CalculateJulianDay(timeUTC);
            var (SelectedAyanmsaDMS, NewCombeAdjust) = GeSelectedAyanmsaDMSandAyanNewCombe(userInfo, julianDay);
            GlobalChartCreatorDataRequest chartData = GetChartData(julianDay, timezone, SelectedAyanmsaDMS);
            #region Bhawa List Logic
            //  List<string> BhawaPlanet_BList = new List<string>();
            var lstBhava = GetBhawaList(userInfo, NewCombeAdjust, julianDay);
            #endregion
            #region Planet List Logic
            var lstPlanetList = GetPlanetList(userInfo, NewCombeAdjust, julianDay);
            #endregion
            SDK_Communicator.CloseSDK();
            tblBhavaAndPlanetRequest objBAP = new tblBhavaAndPlanetRequest();
            objBAP.wrBhavalst = JsonConvert.SerializeObject(lstBhava);
            objBAP.wrPlanetlst = JsonConvert.SerializeObject(lstPlanetList);
            objBAP.wrChartHolderID = Convert.ToInt32(ChartHolderID);
            objBAP.wrCreatedDate = DateTime.Now;
            var IsInserted = _IACRepo.InsertBhavaAndPlanet(objBAP);
            return new Tuple<List<BhavaResponse>, List<PlanetResponse>>(lstBhava, lstPlanetList);
        }

        private double GetTimeZone(ChartHolderResponse userInfo)
        {
            TimeSpan usersTimeZone = TimeSpan.Parse(userInfo.WrTimeZone.Substring(1, userInfo.WrTimeZone.Length - 1));//parameter which is needed for method
            double timezone = usersTimeZone.TotalHours;
            if (userInfo.WrTimeZone[0] == '-')
                timezone *= -1;
            return timezone;
        }
        private (string, double) GeSelectedAyanmsaDMSandAyanNewCombe(ChartHolderResponse userInfo, double julianDay)
        {
            var SelectedAyanmsaDMS = "";
            double AyanNewCombe = 0.0;

            var ayanamsa = Convert.ToInt32(userInfo.WrAyanamsa);
            (string SelectedAyanmsaDMS, double NewCombeAdjust) result;
            if (userInfo.WrAynamsaPolicy == "Nirayana")
            {
                if (ayanamsa == 999)
                {
                    sidmode = SDK_Communicator.SetSidMode(5, 0, 0);
                    tempAyanamshaCalculated = SDK_Communicator.GetAyanamsaUt(julianDay);
                    var ayandays = (Convert.ToDateTime(userInfo.DateDOB).Date - new DateTime(291, 04, 15).Date).TotalDays;
                    AyanNewCombe = (ayandays * (50.2388475 / 365.242375)) / 3600;
                    SelectedAyanmsaDMS = SDK_Communicator.ConvertDegreesToDMS(AyanNewCombe);
                }
                else
                {
                    ayanamshaCalculated = SDK_Communicator.GetAyanamsaUt(julianDay);
                    if (ayanamsa == 255)
                    {
                        int SE_SIDBIT_USER_UT = 1024;
                        sidmode = SDK_Communicator.SetSidMode(ayanamsa + SE_SIDBIT_USER_UT, 23.0, 0);
                    }
                    else
                    {
                        sidmode = SDK_Communicator.SetSidMode(ayanamsa, 0, 0);
                    }
                    SelectedAyanmsaDMS = SDK_Communicator.ConvertDegreesToDMS(ayanamshaCalculated);
                }
            }

            var NewCombeAdjust = tempAyanamshaCalculated - AyanNewCombe;
            result = (SelectedAyanmsaDMS, NewCombeAdjust);
            return result;
        }
        private GlobalChartCreatorDataRequest GetChartData(double julianDay, double timezone, string SelectedAyanmsaDMS)
        {
            var AYANAMSHAS = new int[4] { 1, 3, 5, 999 };
            var chartData = new GlobalChartCreatorDataRequest();

            chartData.txtSideralTime = SDK_Communicator.GetSideralTime(julianDay).ToString();
            chartData.txtSideralTime = SDK_Communicator.ConvertDegreesToDMS(SDK_Communicator.GetSideralTime(julianDay, 0, 0) + timezone);
            string data = Regex.Replace(chartData.txtSideralTime, @"°|'", ":");
            chartData.txtSideralTime = Convert.ToInt32(data.Substring(0, 3)).ToString() + data.Substring(3);
            chartData.txtAyanamsaInDMS = SelectedAyanmsaDMS;
            for (int i = 0; i < AYANAMSHAS.Length; i++)
            {
                string value = CalculateChartData(AYANAMSHAS[i], julianDay);
                PopulateChartData(chartData, AYANAMSHAS[i], value);
            }
            return chartData;
        }
        void PopulateChartData(GlobalChartCreatorDataRequest chartData, int ayanamsa, string value)
        {
            if (ayanamsa == 1)
            {
                chartData.lahiri = value;
            }
            else if (ayanamsa == 3)
            {
                chartData.raman = value;
            }
            else if (ayanamsa == 5)
            {
                chartData.krishnamurti = value;
            }
            else if (ayanamsa == 999)
            {
                chartData.ayanNewCombe = value;
            }
        }
        string CalculateChartData(int ayanamsa, double julianDay)
        {
            sidmode = SDK_Communicator.SetSidMode(ayanamsa, 0, 0);

            double ayanamshaCalculated = SDK_Communicator.GetAyanamsaUt(julianDay);
            // coverted degree
            string ayanamshainDMS1 = SDK_Communicator.ConvertDegreesToDMS(ayanamshaCalculated);

            return ayanamshainDMS1;
        }
        private List<BhavaResponse> GetBhawaList(ChartHolderResponse userInfo, double newCombeAdjust, double julianDay)
        {
            List<BhavaResponse> lstBhava = new List<BhavaResponse>();
            Array arr_Bhawa_NCombe;
            // Get selected House Value from Dropdown ie to say calculation method Eg: "Placidus Method"
            var hsys = userInfo.WrHouseSystem;
            arr_Bhawa = new double[13]; arr_Bhawa_NCombe = new double[13];
            // Get House position /House Cusp Calculation. array for 12 houses.            
            IFlag = SDK_Communicator.GetIFlag(userInfo.WrAynamsaPolicy);

            if (userInfo.WrAynamsaPolicy == "Nirayana")
            {

                if (userInfo.WrAyanamsa == "1")
                {
                    sidmode = SDK_Communicator.SetSidMode(1, 0, 0);
                    Ayanamsha_lahiri = SDK_Communicator.GetAyanamsaUt(julianDay);
                    GenerateArrayBhawa(userInfo, julianDay);
                }
                if (userInfo.WrAyanamsa == "3")
                {
                    sidmode = SDK_Communicator.SetSidMode(3, 0, 0);
                    Ayanamsha_raman = SDK_Communicator.GetAyanamsaUt(julianDay);
                    GenerateArrayBhawa(userInfo, julianDay);
                }

                if (userInfo.WrAyanamsa == "5")
                {
                    sidmode = SDK_Communicator.SetSidMode(5, 0, 0);
                    Ayanamsha_krishnamurti = SDK_Communicator.GetAyanamsaUt(julianDay);
                    GenerateArrayBhawa(userInfo, julianDay);
                }
                if (userInfo.WrAyanamsa == "999")
                {
                    var ayandaysNC = (Convert.ToDateTime(userInfo.DateDOB).Date - new DateTime(291, 04, 15).Date).TotalDays;
                    var AyanNewCombe = (ayandaysNC * (50.2388475 / 365.242375)) / 3600;

                    userInfo.WrAynamsaPolicy = "Sayana";
                    GenerateArrayBhawa(userInfo, julianDay);
                    AdjustBhawaArray(arr_Bhawa, AyanNewCombe);
                }
            }
            else if (userInfo.WrAynamsaPolicy == "Sayana")
            {
                GenerateArrayBhawa(userInfo, julianDay);
            }

            var ayandays = (Convert.ToDateTime(userInfo.DateDOB).Date - new DateTime(291, 04, 15).Date).TotalDays;
            Ayanamsha_NewCombe = (ayandays * (50.2388475 / 365.242375)) / 3600;//23.153986011503552   ///23.153986011503552
            string ayan_NewCombeDMS = SDK_Communicator.ConvertDegreesToDMS(Ayanamsha_NewCombe); //"023 ° 09 ' 14.35"

            // Ayanamsa Calculation for  General Comparison Purposes.
            {
                sidmode = SDK_Communicator.SetSidMode(1, 0, 0);
                ayan_lahiriDMS = SDK_Communicator.ConvertDegreesToDMS(SDK_Communicator.GetAyanamsaUt(julianDay));

                sidmode = SDK_Communicator.SetSidMode(3, 0, 0);
                ayan_ramanDMS = SDK_Communicator.ConvertDegreesToDMS(SDK_Communicator.GetAyanamsaUt(julianDay));

                sidmode = SDK_Communicator.SetSidMode(5, 0, 0);
                ayan_krishnamurtiDMS = SDK_Communicator.ConvertDegreesToDMS(SDK_Communicator.GetAyanamsaUt(julianDay));
            }
            int index = 0;

            List<BhavaResponse> BhawaPlanet_BList = new List<BhavaResponse>();
            //  BhawaPlanet_BList BhawaPlanet_BList
            // Adding to the BhawaPlanet_BList
            foreach (double bhava in arr_Bhawa)
            {
                if (index != 0)
                {
                    double KID = bhava / 30;
                    if (KID - Math.Truncate(KID) > 0)
                    {
                        KID = Math.Ceiling(KID);
                    }
                    KID = KID < 0 || KID > 12 ? 0 : KID;

                    lstBhava.Add(new BhavaResponse() { Num_Index = index, Item_ID = "B", Item_Name = "BH : " + index, Location_DegDig = bhava, KID = Convert.ToInt32(KID) });

                    BhawaPlanet_BList.Add(new BhavaResponse() { Num_Index = index, Item_ID = "B", Item_Name = "BH : " + index, Location_DegDig = bhava, KID = Convert.ToInt32(KID) });
                }
                index++;
            }

            GenerateArrayBhawa(userInfo, julianDay);

            void AdjustBhawaArray(Array arr_Bhawa, double NewCombeAdjust)
            {
                for (int i = 1; i < arr_Bhawa.Length; i++)
                {
                    var value = Convert.ToDouble(arr_Bhawa.GetValue(i));
                    var finalvalue = value - NewCombeAdjust;
                    if (finalvalue < 0)
                    {
                        finalvalue = 360 + finalvalue;
                    }
                    arr_Bhawa.SetValue(finalvalue, i);
                }
            }

            return lstBhava;
        }
        void GenerateArrayBhawa(ChartHolderResponse userInfo, double julianDay)
        {
            var hsys = userInfo.WrHouseSystem;
            IFlag = SDK_Communicator.GetIFlag(userInfo.WrAynamsaPolicy);
            if (userInfo.WrAynamsaPolicy == "Sayana")
            {
                arr_Bhawa = SDK_Communicator.GetHouses_1(julianDay, IFlag, Convert.ToDouble(userInfo.WrLatitude,
                    CultureInfo.InvariantCulture), Convert.ToDouble(userInfo.WrLongitude, CultureInfo.InvariantCulture), Convert.ToChar(hsys));
            }
            else
            {
                arr_Bhawa = SDK_Communicator.GetHousesEx1(julianDay, IFlag, Convert.ToDouble(userInfo.WrLatitude,
                    CultureInfo.InvariantCulture), Convert.ToDouble(userInfo.WrLongitude, CultureInfo.InvariantCulture), Convert.ToChar(hsys));
            }
        }

        private List<PlanetResponse> GetPlanetList(ChartHolderResponse userInfo, double newCombeAdjust, double julianDay)
        {
            string serr = "";
            double planetPosition_DegDigital = 0.0;
            var pname = "";
            var planetName = "";
            List<PlanetResponse> lstPlanetList = new List<PlanetResponse>();
            for (int planet = 0; planet <= 11; planet++)
            {

                if (planet <= 10)
                {
                    //  Getting  Planet positions longitude, latitude, speed in long., speed in lat., and speed in dist
                    string planetPosition = SDK_Communicator.GetPlanetsPositions(julianDay, planet, IFlag, serr);
                    // This is Calculating the set of strings separated by "  :  "
                    string[] StrCount_in_PP = planetPosition.Split(':');

                    //StringBuilder sb = new StringBuilder();
                    planetPosition_DegDigital = Convert.ToDouble(StrCount_in_PP[1]);  // Extract the actual Planet Location (second Set of string)in Deg. Digital

                    if (userInfo.WrAyanamsa == "999")
                    {
                        planetPosition_DegDigital = planetPosition_DegDigital + newCombeAdjust;
                    }

                    // Get planet name
                    planetName = SDK_Communicator.GetPlanetName(planet, pname);
                }

                if (planet == 10) { planetName = "Ragu"; }

                if (planet == 11)
                {
                    planetName = "Kethu";

                    double planetPostion_Kethu = 0;
                    if (planetPosition_DegDigital > Convert.ToDouble(180))
                    {
                        planetPostion_Kethu = planetPosition_DegDigital - 180;
                    }
                    else
                    {
                        planetPostion_Kethu = planetPosition_DegDigital + 180;
                    }

                    planetPosition_DegDigital = planetPostion_Kethu;
                }

                if (planet == 1)
                {
                    AstroGlobalVariables.moonValue = planetPosition_DegDigital;
                }
                int index = 15;
                BhawaPlanet_BList.Add(new BhavaResponse() { Num_Index = index, Item_ID = "P", Item_Name = planetName, Location_DegDig = planetPosition_DegDigital, });

                double KID = planetPosition_DegDigital / 30;
                if (KID - Math.Truncate(KID) > 0)
                {
                    KID = Math.Ceiling(KID);
                }
                KID = KID < 0 || KID > 12 ? 0 : KID;
                // Adding Planet List
                lstPlanetList.Add(new PlanetResponse() { Num_Index = planet, Item_ID = "P", Item_Name = planetName, KID = Convert.ToInt32(KID), Location_DegDig = planetPosition_DegDigital });  // Planet List updated
            }
            return lstPlanetList;
        }

        private List<BhavaAndPlanetResponse> GetBhawaAndPlanetsList(List<BhavaResponse> lstBhava, List<PlanetResponse> lstPlanetList)
        {
            int BavLoc = 0;
            int numindex = 0;
            double PlaLoc = 0.1;
            List<BhavaAndPlanetResponse> lstBhavaAndPlanet = new List<BhavaAndPlanetResponse>();
            for (int i = 0; i < lstBhava.Count; i++)
            {
                BavLoc++;
                lstBhavaAndPlanet.Add(new BhavaAndPlanetResponse()
                {
                    Index = numindex++,
                    KID = lstBhava[i].KID,
                    Item_ID = lstBhava[i].Item_ID,
                    Item_Name = lstBhava[i].Item_Name,
                    Relative_Order = BavLoc.ToString(),
                    Location_DegDig = lstBhava[i].Location_DegDig,
                    Location_DMS = SDK_Communicator.ConvertDegreesToDMS(lstBhava[i].Location_DegDig), //coverted degree
                });
                PlaLoc = 0.1;
                BhavaResponse nextBhava; List<PlanetResponse> planets;
                nextBhava = (i == lstBhava.Count - 1) ? lstBhava[0] : lstBhava[i + 1];  // Command to control the loop and also to fetch next bHava

                if (nextBhava.Location_DegDig < lstBhava[i].Location_DegDig)
                    planets = lstPlanetList.Where(p => p.Location_DegDig >= lstBhava[i].Location_DegDig || (p.Location_DegDig > 0 && p.Location_DegDig < nextBhava.Location_DegDig)).ToList();
                else
                    planets = lstPlanetList.Where(p => (p.Location_DegDig >= lstBhava[i].Location_DegDig) && p.Location_DegDig < nextBhava.Location_DegDig).ToList();

                if (planets.Any())
                {
                    planets.ForEach(plnt =>
                    {
                        lstBhavaAndPlanet.Add(new BhavaAndPlanetResponse()
                        {
                            Index = numindex++,
                            KID = plnt.KID,
                            Item_ID = plnt.Item_ID,
                            Item_Name = plnt.Item_Name,
                            Relative_Order = (BavLoc + PlaLoc).ToString(),
                            Location_DegDig = plnt.Location_DegDig,
                            Location_DMS = SDK_Communicator.ConvertDegreesToDMS(plnt.Location_DegDig), // coverted degree
                        });
                        PlaLoc = PlaLoc + .1;
                    });
                }
            }
            return lstBhavaAndPlanet;
        }
        
        public async Task<ServiceResponse> getVimsoChart(VimsoChartRequest obj)
        {
            ServiceResponse objResponse = new ServiceResponse();
            {
                var objData = await _IACRepo.GetBhavaAndPlanet(Convert.ToInt32(obj.WrChartHolderID));
                obj.WrMoonDegree = JsonConvert.DeserializeObject<List<PlanetResponse>>(objData.wrPlanetlst).Where(t => t.Item_Name == "Moon").FirstOrDefault().Location_DegDig.ToString();
                var objResult = await _IACRepo.getVimsoChart(obj);
                objResponse.success = false;
                if (objResult != null)
                {
                    objResponse.success = true;
                    objResponse.result = JsonConvert.SerializeObject(objResult);
                }
            }
            return objResponse;
        }

        public async Task<ServiceResponse> getVimsoChart_DI_Grp(VimsoChartRequest obj)
        {
            ServiceResponse objResponse = new ServiceResponse();
            {
                var objData = await _IACRepo.GetBhavaAndPlanet(Convert.ToInt32(obj.WrChartHolderID));
                obj.WrMoonDegree = JsonConvert.DeserializeObject<List<PlanetResponse>>(objData.wrPlanetlst).Where(t => t.Item_Name == "Moon").FirstOrDefault().Location_DegDig.ToString();
                var objResult = await _IACRepo.getVimsoChart_DI_Grp(obj);
                objResponse.success = false;
                if (objResult != null)
                {
                    objResponse.success = true;
                    objResponse.result = JsonConvert.SerializeObject(objResult);
                }
            }
            return objResponse;
        }
    }
}
