using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPDCA1
{
    class Station
    {
        string StationName;
        bool isInterchange = false;

        private List<string> StationCode = new List<string>();

        public Station(string StationCd, string StationName)
        {
            this.StationName = StationName;
            StationCode.Add(StationCd);
        }

        public Station() { }

        public void setStationCode(List<string> StationCd)
        {
            this.StationCode = StationCd;
        }
        public List<string> getStationCode()
        {
            return StationCode;
        }
        public void setStationName(string StationName)
        {
            this.StationName = StationName;
        }
        public string getStationName()
        {
            return StationName;
        }

        public void setStationInterchangeStatus(bool StationInterchange)
        {
            this.isInterchange = StationInterchange;
        }
        public bool getStationInterchangeStatus()
        {
            return isInterchange;
        }
    }
}
