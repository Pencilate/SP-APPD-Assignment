using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace APPDCA1
{
    public class FileIO
    {
        public static void textMRTFileReaderToDB(string FilePath)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        connection.Open();
                        cmd.Connection = connection;
                        string StationName;
                        using (StreamReader reader = new StreamReader(FilePath))
                        {
                            string lineData;
                            while ((lineData = reader.ReadLine()) != null)
                            {
                                string LineCd = "";
                                switch (lineData)
                                {
                                    case "(start)":
                                        string LineStationCdStr = reader.ReadLine();
                                        LineCd = LineStationCdStr.Substring(0, 2);
                                        Console.WriteLine(LineCd);

                                        cmd.CommandText = "INSERT INTO LineCdRef (LineCd) VALUES (@LineCd)";

                                        cmd.Parameters.Add("@LineCd");
                                        cmd.Parameters["@LineCd"].Value = LineCd;
                                        cmd.ExecuteNonQuery();

                                        if (LineCd.Equals("CG"))
                                        { //Special Case for Tanah Merah
                                            cmd.CommandText = "INSERT INTO Station (LineCd, StationCode, StationName) VALUES (@LineCd,@StatCd,@StatName)";

                                            cmd.Parameters.Add("@LineCd");
                                            cmd.Parameters.Add("@StatCd");
                                            cmd.Parameters.Add("@StatName");

                                            cmd.Parameters["@LineCd"].Value = LineCd;
                                            cmd.Parameters["@StatCd"].Value = "CG0";
                                            cmd.Parameters["Statname"].Value = "Tanah Merah";

                                            cmd.ExecuteNonQuery();
                                        }

                                        StationName = reader.ReadLine();

                                        cmd.CommandText = "INSERT INTO Station (LineCd, StationCode, StationName) VALUES (@LineCd,@StatCd,@StatName)";

                                        cmd.Parameters.Add("@LineCd");
                                        cmd.Parameters.Add("@StatCd");
                                        cmd.Parameters.Add("@StatName");

                                        cmd.Parameters["@LineCd"].Value = LineCd;
                                        cmd.Parameters["@StatCd"].Value = LineStationCdStr;
                                        cmd.Parameters["Statname"].Value = StationName;

                                        cmd.ExecuteNonQuery();
                                        break;
                                    case "(end)":
                                        break;
                                    default:
                                        StationName = reader.ReadLine();

                                        StationName = reader.ReadLine();

                                        cmd.CommandText = "INSERT INTO Station (LineCd, StationCode, StationName) VALUES (@LineCd,@StatCd,@StatName)";

                                        cmd.Parameters.Add("@LineCd");
                                        cmd.Parameters.Add("@StatCd");
                                        cmd.Parameters.Add("@StatName");

                                        cmd.Parameters["@LineCd"].Value = LineCd;
                                        cmd.Parameters["@StatCd"].Value = lineData;
                                        cmd.Parameters["Statname"].Value = StationName;

                                        cmd.ExecuteNonQuery();
                                        break;
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Console.WriteLine("Error Retriving Line Information");
                    }
                    finally
                    {
                        connection.Close();
                        Console.WriteLine("Connection closed");
                    }
                }
            }
        }

        public static void textFareFileReaderToDB(string FilePath)
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string fareData;
                List<string> startStatCdList= new List<string>();
                List<string> endStatCdList = new List<string>();
                while ((fareData = reader.ReadLine()) != null)
                {
                    int spaceIndex = fareData.IndexOf(" ");
                    string startStatCd = fareData.Substring(0, 4).Trim();
                    string endStatCd = fareData.Substring(spaceIndex).Trim();
                    int slashIndex;
                    if (startStatCd.IndexOf("/") == -1)
                    {
                        if (endStatCd.IndexOf("/") == -1)
                        {
                            double cardFare = double.Parse(reader.ReadLine().TrimStart('$'));
                            double standardTicket = double.Parse(reader.ReadLine().TrimStart('$'));
                            int timeTaken = int.Parse(reader.ReadLine());
                            Console.WriteLine("{0}\n{1}\n{2}\n{3}\n{4}", startStatCd, endStatCd, cardFare, standardTicket, timeTaken);
                        }
                        else
                        {
                            slashIndex = endStatCd.IndexOf("/");
                            while (slashIndex != -1)
                            {
                                endStatCdList.Add(endStatCd.Substring(0, slashIndex));
                                string endStat = endStatCd.Substring(slashIndex);
                                slashIndex = endStatCd.IndexOf("/");
                            }
                            double cardFare = double.Parse(reader.ReadLine().TrimStart('$'));
                            double standardTicket = double.Parse(reader.ReadLine().TrimStart('$'));
                            int timeTaken = int.Parse(reader.ReadLine());
                            Console.WriteLine("{0}\n{1}\n{2}\n{3}\n{4}", startStatCd, endStatCd, cardFare, standardTicket, timeTaken);
                        }
                    }
                    else if (endStatCd.IndexOf("/") == -1)
                    {
                        slashIndex = startStatCd.IndexOf("/");
                        while (slashIndex != -1)
                        {
                            startStatCdList.Add(startStatCd.Substring(0, slashIndex));
                            string startStat = startStatCd.Substring(slashIndex);
                            slashIndex = startStatCd.IndexOf("/");
                        }
                        double cardFare = double.Parse(reader.ReadLine().TrimStart('$'));
                        double standardTicket = double.Parse(reader.ReadLine().TrimStart('$'));
                        int timeTaken = int.Parse(reader.ReadLine());
                        Console.WriteLine("{0}\n{1}\n{2}\n{3}\n{4}", startStatCd, endStatCd, cardFare, standardTicket, timeTaken);
                    }
                }
            }
        }
    }
}
