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
        private List<Station> StationLine = new List<Station>();

        public Line(string lineCd) //Line constructor with parameters
        {
            this.lineCd = lineCd;
        }
        public Line() //Line constructor
        {

        }

        public void AddStationToLine(string StationCd, string StationName) //Adds a Station to the Line
        {
            StationLine.Add(new Station(StationCd, StationName));
        }

        internal Station getStation(int index) //Returns station object based on its index 
        {
            return StationLine[index];
        }

        internal List<Station> getStationList() //Returns Station Line
        {
            return StationLine;
        }

        public string LineCd //Line Code property of Line object
        {
            get { return lineCd; } //get method
            set { this.lineCd = value; } //set method
        }

    }
}
