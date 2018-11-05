using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace APPDCA1
{
    public class Line
    {
        string LineCd;
        List<Station> StationLine = new List<Station>();

        public Line(string LineCd)
        {
            this.LineCd = LineCd;
        }
        public Line()
        {

        }

        public void AddStationToLine(string StationCd, string StationName)
        {
            StationLine.Add(new Station(StationCd, StationName));
        }

        internal Station getStation(int index)
        {
            return StationLine[index];
        }

        internal List<Station> getStationList()
        {
            return StationLine;
        }

        public void setLineCd(string LineCd)
        {
            this.LineCd = LineCd;
        }
        public string getLineCd()
        {
            return LineCd;
        }
    }
}
