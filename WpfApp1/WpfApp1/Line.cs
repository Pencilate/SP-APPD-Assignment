using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Line //Line class
    {
        private string lineCd;
        private List<Station> stationLine = new List<Station>();

        public Line(string lineCd) //Line constructor with parameters
        {
            this.lineCd = lineCd;
        }
        public Line() { } //Line constructor

        public void AddStationToLine(string StationCd, string StationName) //Adds a Station to the Line
        {
            stationLine.Add(new Station(StationCd, StationName));
        }

        public List<Station> StationList //Returns Station Line
        {
            get { return stationLine; }
        }

        public string LineCd //Line Code property of Line object
        {
            get { return lineCd; } //get method
            set { this.lineCd = value; } //set method
        }
    }
}
