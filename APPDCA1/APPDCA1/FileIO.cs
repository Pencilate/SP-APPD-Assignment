using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace APPDCA1
{
    public class FileIO
    {
        public static List<Line> textFileReader(string FilePath)
        {
            List<Line> MRT = new List<Line>();
            string StationName;
            int LineNo = 0;
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string lineData;
                while ((lineData = reader.ReadLine()) != null)
                {
                    switch (lineData)
                    {
                        case "(start)":
                            string LineStationCdStr = reader.ReadLine();
                            string LineCd = LineStationCdStr.Substring(0, 2);
                            Console.WriteLine(LineCd);

                            MRT.Add(new Line(LineCd));
                            if (LineCd.Equals("CG"))
                            { //Special Case for Tanah Merah
                                MRT[LineNo].AddStationToLine("CG0", "Tanah Merah");
                            }
                            StationName = reader.ReadLine();
                            MRT[LineNo].AddStationToLine(LineStationCdStr, StationName);
                            break;
                        case "(end)":
                            LineNo++;
                            break;
                        default:
                            StationName = reader.ReadLine();
                            MRT[LineNo].AddStationToLine(lineData, StationName);
                            break;
                    }
                }

            }

            //Comparing station to set them as interchange
            for (int count = (MRT.Count - 1); count > 0; count--)
            {
                int CurrentLine = count;
                int ComparedLine = count - 1;
                for (int i = ComparedLine; i >= 0; i--)
                {

                    for (int j = 0; j < MRT[CurrentLine].StationList.Count(); j++)
                    {

                        for (int h = 0; h < MRT[i].StationList.Count(); h++)
                        {
                            //Compare name of stations between CurruntLine and ComparedLine
                            Console.WriteLine("{0} {1} VS {2} {3}", MRT[CurrentLine].LineCd, MRT[CurrentLine].StationList[j].StationName, MRT[i].LineCd, MRT[i].StationList[h].StationName);
                            if (MRT[CurrentLine].StationList[j].StationName.Equals(MRT[i].StationList[h].StationName))
                            {
                                //if the station names match,                              
                                MRT[CurrentLine].StationList[j].IsInterchange = true;
                                //set varible to identify it as a interchange as true


                                Console.WriteLine("YES");
                                foreach (string str in MRT[CurrentLine].StationList[j].StationCode)
                                {
                                    Console.Write(str + " ");
                                }
                                Console.WriteLine();
                                foreach (string str in MRT[i].StationList[h].StationCode)
                                {
                                    Console.Write(str + " ");
                                }
                                Console.WriteLine();

                                if ((MRT[CurrentLine].StationList[j].StationCode) != (MRT[i].StationList[h].StationCode))
                                {
                                    MRT[CurrentLine].StationList[j].StationCode.AddRange(MRT[i].StationList[h].StationCode);
                                }

                                foreach (string str in MRT[CurrentLine].StationList[j].StationCode)
                                {
                                    Console.Write(str + " ");
                                }
                                Console.WriteLine();

                                MRT[i].StationList[h] = MRT[CurrentLine].StationList[j];

                            }//end if

                        }
                    }

                }

            }


            //Old Code Pls Review and remove
            //                   for(int i=0;i<MRT.get(0).StationList.size();i++){
            //              if(MRT.get(0).getStation(i).getStationInterchangeStatus()){
            //                  System.out.println(MRT.get(0).getStation(i).getStationName());
            //                  int size =MRT.get(0).getStation(i).getStationCode().size();
            //                  for(int count =0; count<size;count++){
            //                      System.out.println(MRT.get(0).getStation(i).getStationCode().get(count));
            //                  }
            //              }
            //          }

            //Console.WriteLine(MRT[0].StationList[0].IsInterchange);
            //Console.WriteLine(MRT[2].StationList[3].IsInterchange);

            return MRT;
        }

        public string FilePath = "..\\..\\resources\\fares.txt";
        public static void textFileReaderFare(string FilePath)
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                //string faresData;
                //while ((faresData = reader.ReadLine()) != null)
                string StartStationCd = reader.ReadLine().Substring(0, 4);
                string EndStationCd = reader.ReadLine().Substring(5,);
            }
        }
    }
}
