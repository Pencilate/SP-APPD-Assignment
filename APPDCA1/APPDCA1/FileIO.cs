﻿using System;
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
        //Make sure to check the connection string
        private const string connectionString = "Data Source=DIT-NB1828823\\SQLEXPRESS; database=APPDCADB; integrated security = true;";
        public static void textMRTFileReaderToDB(string FilePath)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();
                        cmd.Connection = connection;

                        cmd.CommandText = "DELETE FROM Fare";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "DELETE FROM Station";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "DELETE FROM LineCdRef";
                        cmd.ExecuteNonQuery();
                       

                        string StationName;
                        using (StreamReader reader = new StreamReader(FilePath))
                        {
                            string lineData;

                            cmd.Parameters.Add("@LineCd",SqlDbType.VarChar,2);
                            cmd.Parameters.Add("@StatCd",SqlDbType.VarChar,4);
                            cmd.Parameters.Add("@StatName",SqlDbType.VarChar,255);

                            string LineCd = "";
                            while ((lineData = reader.ReadLine()) != null)
                            {
                                switch (lineData)
                                {
                                    case "(start)":
                                        string LineStationCdStr = reader.ReadLine();
                                        LineCd = LineStationCdStr.Substring(0, 2);
                                        Console.WriteLine(LineCd);

                                        cmd.CommandText = "INSERT INTO LineCdRef (Line_Code) VALUES (@LineCd)";


                                        cmd.Parameters["@LineCd"].Value = LineCd;
                                        cmd.Parameters["@StatCd"].Value = "";
                                        cmd.Parameters["@StatName"].Value = "";
                                        cmd.ExecuteNonQuery();

                                        if (LineCd.Equals("CG"))
                                        { //Special Case for Tanah Merah
                                            cmd.CommandText = "INSERT INTO Station (Line_Code, Station_Code, Station_Name) VALUES (@LineCd,@StatCd,@StatName)";

                                            cmd.Parameters["@LineCd"].Value = LineCd;
                                            cmd.Parameters["@StatCd"].Value = "CG0";
                                            cmd.Parameters["@StatName"].Value = "Tanah Merah";

                                            cmd.ExecuteNonQuery();
                                        }

                                        StationName = reader.ReadLine();

                                        cmd.CommandText = "INSERT INTO Station (Line_Code, Station_Code, Station_Name) VALUES (@LineCd,@StatCd,@StatName)";

                                        cmd.Parameters["@LineCd"].Value = LineCd;
                                        cmd.Parameters["@StatCd"].Value = LineStationCdStr;
                                        cmd.Parameters["@StatName"].Value = StationName;

                                        cmd.ExecuteNonQuery();
                                        break;
                                    case "(end)":
                                        break;
                                    default:
                                        Console.WriteLine("Default case");
                                        Console.WriteLine(LineCd);
                                        StationName = reader.ReadLine();

                                        cmd.CommandText = "INSERT INTO Station (Line_Code, Station_Code, Station_Name) VALUES (@LineCd,@StatCd,@StatName)";

                                        cmd.Parameters["@LineCd"].Value = LineCd;
                                        cmd.Parameters["@StatCd"].Value = lineData;
                                        cmd.Parameters["@StatName"].Value = StationName;

                                        cmd.ExecuteNonQuery();
                                        break;
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Retriving Line Information\nError Details:{0}", ex);
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
            List<string> stationCdBlacklsit = new List<string>() {"PTC", "STC" };
            using (SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();
                        cmd.Connection = connection;

                        cmd.CommandText = "DELETE FROM Fare";
                        cmd.ExecuteNonQuery();

                        using (StreamReader reader = new StreamReader(FilePath))
                        {
                            string fareData;
                            List<string> startStatCdList = new List<string>();
                            List<string> endStatCdList = new List<string>();
                            string startStat = string.Empty;
                            string endStat = string.Empty;

                            cmd.Parameters.Add("@StartStatCd",SqlDbType.VarChar,4);
                            cmd.Parameters.Add("@EndStatCd", SqlDbType.VarChar, 4);
                            cmd.Parameters.Add("@CardFare",SqlDbType.Money);
                            cmd.Parameters.Add("@TicketFare",SqlDbType.Money);
                            cmd.Parameters.Add("@JourneyDuration",SqlDbType.Int);

                            while ((fareData = reader.ReadLine()) != null)
                            {
                                int spaceIndex = fareData.IndexOf(" ");
                                string startStatCd = fareData.Substring(0, spaceIndex).Trim();
                                string endStatCd = fareData.Substring(spaceIndex).Trim();
                                int slashIndex;
                                Console.Write("{0}-{1}|Space:{2}|", startStatCd, endStatCd, spaceIndex);
                                //if (startStatCd.IndexOf("/") == -1)
                                //{
                                //    if (endStatCd.IndexOf("/") == -1)
                                //    {
                                //        double cardFare = double.Parse(reader.ReadLine().TrimStart('$'));
                                //        double standardTicket = double.Parse(reader.ReadLine().TrimStart('$'));
                                //        int timeTaken = int.Parse(reader.ReadLine());
                                //        Console.WriteLine("{0}\n{1}\n{2}\n{3}\n{4}", startStatCd, endStatCd, cardFare, standardTicket, timeTaken);
                                //    }
                                //    else
                                //    {
                                //        slashIndex = endStatCd.IndexOf("/");
                                //        while (slashIndex != -1)
                                //        {
                                //            firstEndStat = endStatCd.Substring(0, slashIndex);
                                //            endStatCdList.Add(firstEndStat);
                                //            endStat = endStatCd.Substring(slashIndex+1);
                                //            slashIndex = endStat.IndexOf("/");
                                //        }
                                //        double cardFare = double.Parse(reader.ReadLine().TrimStart('$'));
                                //        double standardTicket = double.Parse(reader.ReadLine().TrimStart('$'));
                                //        int timeTaken = int.Parse(reader.ReadLine());
                                //        Console.WriteLine("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", startStatCd, firstEndStat, endStat, cardFare, standardTicket, timeTaken);
                                //    }
                                //}
                                //else if (endStatCd.IndexOf("/") == -1)
                                //{
                                //    slashIndex = startStatCd.IndexOf("/");
                                //    while (slashIndex != -1)
                                //    {
                                //        startStatCdList.Add(startStatCd.Substring(0, slashIndex));
                                //        startStat = startStatCd.Substring(slashIndex);
                                //        slashIndex = startStat.IndexOf("/");
                                //    }
                                //    double cardFare = double.Parse(reader.ReadLine().TrimStart('$'));
                                //    double standardTicket = double.Parse(reader.ReadLine().TrimStart('$'));
                                //    int timeTaken = int.Parse(reader.ReadLine());
                                //    Console.WriteLine("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", startStatCdList[0], startStat, endStatCd, cardFare, standardTicket, timeTaken);
                                //}
                                slashIndex = startStatCd.IndexOf("/");
                                Console.Write("START:|{0}|", slashIndex);
                                while (slashIndex != -1)
                                {
                                    startStatCdList.Add(startStatCd.Substring(0, slashIndex));
                                    startStatCd = startStatCd.Substring(slashIndex + 1);
                                    slashIndex = startStatCd.IndexOf("/");
                                    Console.Write("{0}|", slashIndex);
                                }
                                startStatCdList.Add(startStatCd);

                                slashIndex = endStatCd.IndexOf("/");
                                Console.Write("END:|{0}|", slashIndex);
                                while (slashIndex != -1)
                                {
                                    endStatCdList.Add(endStatCd.Substring(0, slashIndex));
                                    endStatCd = endStatCd.Substring(slashIndex + 1);
                                    slashIndex = endStatCd.IndexOf("/");
                                    Console.Write("{0}|", slashIndex);
                                }
                                endStatCdList.Add(endStatCd);
                                double cardFare = double.Parse(reader.ReadLine().TrimStart('$'));
                                double standardTicket = double.Parse(reader.ReadLine().TrimStart('$'));
                                int timeTaken = int.Parse(reader.ReadLine());
                                Console.WriteLine("{0},{1}", startStatCdList.Count, endStatCdList.Count);
                                foreach (string ssC in startStatCdList)
                                {
                                    if (stationCdBlacklsit.Contains(ssC))
                                    {
                                        continue;
                                    }
                                    foreach (string esC in endStatCdList)
                                    {
                                        if (stationCdBlacklsit.Contains(esC))
                                        {
                                            continue;
                                        }
                                        Console.WriteLine("{0}-{1}-{2}-{3}-{4}", ssC, esC, cardFare, standardTicket, timeTaken);
                                        //INSERT INSERT SQL STATEMENTS HERE
                                        cmd.CommandText = "BEGIN TRY INSERT INTO Fare (Start_Station_Code,End_Station_Code,Card_Fare,Ticket_Fare,Journey_Duration) VALUES (@StartStatCd,@EndStatCd,@CardFare,@TicketFare,@JourneyDuration) END TRY BEGIN CATCH END CATCH";

                                        //Forward Direction
                                        cmd.Parameters["@StartStatCd"].Value = ssC;
                                        cmd.Parameters["@EndStatCd"].Value = esC;
                                        cmd.Parameters["@CardFare"].Value = cardFare;
                                        cmd.Parameters["@TicketFare"].Value = standardTicket;
                                        cmd.Parameters["@JourneyDuration"].Value = timeTaken;

                                        cmd.ExecuteNonQuery();
                                        //Backward Direction
                                        cmd.Parameters["@StartStatCd"].Value = esC;
                                        cmd.Parameters["@EndStatCd"].Value = ssC;

                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                Console.WriteLine("End stat");
                                startStatCdList.Clear();
                                endStatCdList.Clear();


                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Unable to Insert Fare Infomation\nError Details:{0}", ex);
                    }
                    finally
                    {
                        connection.Close();
                        Console.WriteLine("Connection closed");
                    }
                }

            }
        }
    }
}
