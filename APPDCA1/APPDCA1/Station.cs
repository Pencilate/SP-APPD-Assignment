using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPDCA1
{
    public class Station
    {
        private string stationName;
        private bool isInterchange = false;
        private int graphIndex = -1;

        private List<string> stationCode = new List<string>();

        public Station(string stationCd, string stationName)
        {
            this.stationName = stationName;
            StationCode.Add(stationCd);
        }

        public Station() { }

        public int GraphIndex
        {
            get { return graphIndex; }
            set { this.graphIndex = value; }
        }

        public List<string> StationCode
        {
            get { return stationCode; }
            set { this.stationCode = value; }
        }

        public string StationName
        {
            get { return stationName; }
            set { this.stationName = value; }
        }

        public bool IsInterchange
        {
            get { return isInterchange; }
            set { this.isInterchange = value; }
        }
    }
}
