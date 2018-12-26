using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Station //Station class
    {
        private string stationName;
        private bool isInterchange = false;

        private List<string> stationCode = new List<string>();

        public Station(string stationCd, string stationName) //Station constructor with parameters
        {
            this.stationName = stationName;
            StationCode.Add(stationCd);
        }

        public Station() { } //Station constructor

        public List<string> StationCode //StationCode property of Station object
        {
            get { return stationCode; } //get method
            set { this.stationCode = value; } //set method
        }

        public string StationName //StationName property of Station object
        {
            get { return stationName; } //get method
            set { this.stationName = value; } //set method
        }

        public bool IsInterchange //Determines whether Station object is an interchange or not
        {
            get { return isInterchange; } //get method
            set { this.isInterchange = value; } //set method
        }
    }
}
