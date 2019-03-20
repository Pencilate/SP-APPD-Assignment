using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace WpfApp1
{
    public class FileIO //File input and output 
    {
        //Make sure to check the connection string
        private const string connectionString = "Data Source=DIT-NB1828823\\SQLEXPRESS; database=APPDCADB; integrated security = true;";
        //private const string connectionString = "Data Source=DIT-NB1829233\\SQLEXPRESS; database=APPDCADB; integrated security = true;";
        public static void textMRTFileReaderToDB(string FilePath) //Read mrt text file and store in Database
        {
            List<string> stationCdMRTBlacklsit = new List<string>() { "CC18", "DT1", "DT36", "DT37"}; //blacklist
            using (SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString; //set connection string
                        connection.Open(); //open connection
                        cmd.Connection = connection;

                        cmd.CommandText = "DELETE FROM Fare"; //sql statement
                        cmd.ExecuteNonQuery(); //execute sql statement
                        cmd.CommandText = "DELETE FROM Station"; //sql statement
                        cmd.ExecuteNonQuery(); //execute sql statement
                        cmd.CommandText = "DELETE FROM LineCdRef"; //sql statement
                        cmd.ExecuteNonQuery(); //execute sql statement


                        string StationName; //string 
                        using (StreamReader reader = new StreamReader(FilePath)) //streamreader
                        {
                            string lineData;
                            int counter = 1;
                            cmd.Parameters.Add("@LineCd", SqlDbType.VarChar, 2); //add parameters
                            cmd.Parameters.Add("@StatCd", SqlDbType.VarChar, 4); //add parameters
                            cmd.Parameters.Add("@StatName", SqlDbType.VarChar, 255); //add parameters
                            cmd.Parameters.Add("@Id", SqlDbType.Int); //add parameters
                            string LineCd = "";
                            while ((lineData = reader.ReadLine()) != null)
                            {
                                switch (lineData) //switch 
                                {
                                    case "(start)":
                                        string LineStationCdStr = reader.ReadLine();
                                        LineCd = LineStationCdStr.Substring(0, 2);
                                        Console.WriteLine(LineCd);

                                        cmd.CommandText = "INSERT INTO LineCdRef (Line_Code) VALUES (@LineCd)"; //sql statement


                                        cmd.Parameters["@LineCd"].Value = LineCd; //set value of parameters
                                        cmd.Parameters["@StatCd"].Value = ""; //set value of parameters
                                        cmd.Parameters["@StatName"].Value = ""; //set value of parameters
                                        cmd.Parameters["@Id"].Value = 0; //set value of parameters
                                        cmd.ExecuteNonQuery(); //execute sql statement

                                        if (LineCd.Equals("CG"))
                                        { //Special Case for Tanah Merah
                                            cmd.CommandText = "INSERT INTO Station (Line_Code, Id, Station_Code, Station_Name) VALUES (@LineCd,@Id,@StatCd,@StatName)"; //sql statement

                                            cmd.Parameters["@LineCd"].Value = LineCd; //set value of parameters
                                            cmd.Parameters["@StatCd"].Value = "CG0"; //set value of parameters
                                            cmd.Parameters["@StatName"].Value = "Tanah Merah"; //set value of parameters
                                            cmd.Parameters["@Id"].Value = counter; //set value of parameters
                                            counter++; //increase counter by 1 
                                            cmd.ExecuteNonQuery(); //execute sql statement
                                        }

                                        StationName = reader.ReadLine();
                                        if (stationCdMRTBlacklsit.Contains(lineData))
                                        {
                                            continue;
                                        }
                                        cmd.CommandText = "INSERT INTO Station (Line_Code, Id, Station_Code, Station_Name) VALUES (@LineCd,@Id,@StatCd,@StatName)"; //sql statement
                                         
                                        cmd.Parameters["@LineCd"].Value = LineCd; //set value of parameters
                                        cmd.Parameters["@StatCd"].Value = LineStationCdStr; //set value of parameters
                                        cmd.Parameters["@StatName"].Value = StationName; //set value of parameters
                                        cmd.Parameters["@Id"].Value = counter; //set value of parameters
                                        counter++; //increase counter
                                        cmd.ExecuteNonQuery(); //execute sql statement
                                        break;
                                    case "(end)":
                                        break;
                                    default: //default case
                                        Console.WriteLine("Default case");
                                        Console.WriteLine(LineCd);
                                        StationName = reader.ReadLine();

                                        if (stationCdMRTBlacklsit.Contains(lineData))
                                        {
                                            continue;
                                        }
                                        cmd.CommandText = "INSERT INTO Station (Line_Code, Id, Station_Code, Station_Name) VALUES (@LineCd,@Id,@StatCd,@StatName)"; //sql statement

                                        cmd.Parameters["@LineCd"].Value = LineCd; //set value of parameters
                                        cmd.Parameters["@StatCd"].Value = lineData; //set value of parameters
                                        cmd.Parameters["@StatName"].Value = StationName; //set value of parameters
                                        cmd.Parameters["@Id"].Value = counter; //set value of parameters
                                        counter++; //increase counter by 1
                                        cmd.ExecuteNonQuery(); //execute sql statement
                                        break;
                                }
                            }

                        }
                    }
                    catch (Exception ex) //catch exception error
                    {
                        Console.WriteLine("Error Retriving Line Information\nError Details:{0}", ex);
                    }
                    finally
                    {
                        connection.Close(); //close connection
                        Console.WriteLine("Connection closed");
                    }
                }
            }
        }

        public static void textFareFileReaderToDB(string FilePath) //read fare text file and store it in Database
        {

            List<string> stationCdFareBlacklsit = new List<string>() { "PTC", "STC","CE1","CE2" }; //new list
            int counter = 0;
            using (SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString; //set connection string
                        connection.Open(); //open connection
                        cmd.Connection = connection;

                        cmd.CommandText = "DELETE FROM Fare"; //sql statement
                        cmd.ExecuteNonQuery(); //execute sql statement

                        using (StreamReader reader = new StreamReader(FilePath))
                        {
                            string fareData;
                            List<string> startStatCdList = new List<string>(); //new list
                            List<string> endStatCdList = new List<string>(); //new list
                            string startStat = string.Empty;
                            string endStat = string.Empty;

                            cmd.Parameters.Add("@StartStatCd", SqlDbType.VarChar, 4); //add sql parameters
                            cmd.Parameters.Add("@EndStatCd", SqlDbType.VarChar, 4); //add sql parameters
                            cmd.Parameters.Add("@CardFare", SqlDbType.Money); //add sql parameters
                            cmd.Parameters.Add("@TicketFare", SqlDbType.Money); //add sql parameters
                            cmd.Parameters.Add("@JourneyDuration", SqlDbType.Int); //add sql parameters

                            while ((fareData = reader.ReadLine()) != null)
                            {
                                int spaceIndex = fareData.IndexOf(" "); //find index of space
                                string startStatCd = fareData.Substring(0, spaceIndex).Trim(); //substring from start to spaceindex and trim
                                string endStatCd = fareData.Substring(spaceIndex).Trim(); //substring from spaceindex and trim
                                int slashIndex;
                                //Console.Write("{0}-{1}|Space:{2}|", startStatCd, endStatCd, spaceIndex);
                                slashIndex = startStatCd.IndexOf("/"); //set slash index to index of slash
                                //Console.Write("START:|{0}|", slashIndex);
                                while (slashIndex != -1) //while slashindex is not equals to -1 (while slashindex is still found)
                                {
                                    startStatCdList.Add(startStatCd.Substring(0, slashIndex)); //add startstatcd to list
                                    startStatCd = startStatCd.Substring(slashIndex + 1); //substring startstatcd
                                    slashIndex = startStatCd.IndexOf("/"); //check for slashindex again
                                    //Console.Write("{0}|", slashIndex);
                                }
                                startStatCdList.Add(startStatCd); //add startstatcd to list

                                slashIndex = endStatCd.IndexOf("/"); //set slash index to index of slash
                                //Console.Write("END:|{0}|", slashIndex);
                                while (slashIndex != -1) //while slashindex is not equals to -1( while slashindex is still found)
                                {
                                    endStatCdList.Add(endStatCd.Substring(0, slashIndex)); //add endstatcd to list
                                    endStatCd = endStatCd.Substring(slashIndex + 1); //substring endstatcd
                                    slashIndex = endStatCd.IndexOf("/"); //check for slashindex again
                                    //Console.Write("{0}|", slashIndex);
                                }
                                endStatCdList.Add(endStatCd); //add endstatcd to list
                                double cardFare = double.Parse(reader.ReadLine().TrimStart('$')); //extract cardfare
                                double standardTicket = double.Parse(reader.ReadLine().TrimStart('$')); //extract standard ticket fare
                                int timeTaken = int.Parse(reader.ReadLine()); //extract timetaken
                               // Console.WriteLine("{0},{1}", startStatCdList.Count, endStatCdList.Count);
                                foreach (string ssC in startStatCdList) //foreach loop
                                {
                                    if (stationCdFareBlacklsit.Contains(ssC))
                                    {
                                        continue;
                                    }
                                    foreach (string esC in endStatCdList) //foreach loop
                                    {
                                        if (stationCdFareBlacklsit.Contains(esC))
                                        {
                                            continue;
                                        }
                                        //Console.WriteLine("{0}-{1}-{2}-{3}-{4}", ssC, esC, cardFare, standardTicket, timeTaken);
                                        //INSERT INSERT SQL STATEMENTS HERE                                        
                                        cmd.CommandText = "INSERT INTO Fare (Start_Station_Code,End_Station_Code,Card_Fare,Ticket_Fare,Journey_Duration) SELECT @StartStatCd,@EndStatCd,@CardFare,@TicketFare,@JourneyDuration WHERE NOT EXISTS ( SELECT * FROM Fare WHERE Start_Station_Code = @StartStatCd AND End_Station_Code = @EndStatCd)"; //sql insert statement
                                        //Forward Direction
                                        cmd.Parameters["@StartStatCd"].Value = ssC; //set value of parameters
                                        cmd.Parameters["@EndStatCd"].Value = esC; //set value of parameters
                                        cmd.Parameters["@CardFare"].Value = cardFare; //set value of parameters
                                        cmd.Parameters["@TicketFare"].Value = standardTicket; //set value of parameters
                                        cmd.Parameters["@JourneyDuration"].Value = timeTaken; //set value of parameters

                                        cmd.ExecuteNonQuery(); //execute sql statement
                                        counter++;
                                        //Backward Direction
                                        cmd.Parameters["@StartStatCd"].Value = esC; //set value of parameters
                                        cmd.Parameters["@EndStatCd"].Value = ssC; //set value of parameters

                                        cmd.ExecuteNonQuery(); //execute sql statement
                                        counter++; //increase counter by 1
                                    }
                                }

                                //Console.WriteLine("End stat");
                                startStatCdList.Clear(); //clear list
                                endStatCdList.Clear(); //clear list


                            }
                            //SPECIAL CASES
                           cmd.CommandText = "INSERT INTO Fare (Start_Station_Code,End_Station_Code,Card_Fare,Ticket_Fare,Journey_Duration) VALUES (@StartStatCd,@EndStatCd,@CardFare,@TicketFare,@JourneyDuration)"; //sql statement
                            cmd.Parameters["@StartStatCd"].Value = "DT1";//set value of parameters
                            cmd.Parameters["@EndStatCd"].Value = "DT2";//set value of parameters
                            cmd.Parameters["@CardFare"].Value = 0.83;//set value of parameters
                            cmd.Parameters["@TicketFare"].Value = 1.50;//set value of parameters
                            cmd.Parameters["@JourneyDuration"].Value = 6;//set value of parameters
                            cmd.ExecuteNonQuery(); //execute sql statement
                            cmd.Parameters["@StartStatCd"].Value = "DT2";//set value of parameters
                            cmd.Parameters["@EndStatCd"].Value = "DT1";//set value of parameters
                            cmd.ExecuteNonQuery(); //execute sql statement
                            cmd.Parameters["@StartStatCd"].Value = "NS3";//set value of parameters
                            cmd.Parameters["@EndStatCd"].Value = "NS4";//set value of parameters
                            cmd.Parameters["@CardFare"].Value = 0.93;//set value of parameters
                            cmd.Parameters["@TicketFare"].Value = 1.70;//set value of parameters
                            cmd.Parameters["@JourneyDuration"].Value = 7;//set value of parameters
                            cmd.ExecuteNonQuery(); //execute sql statement
                            cmd.Parameters["@StartStatCd"].Value = "NS4";//set value of parameters
                            cmd.Parameters["@EndStatCd"].Value = "NS3";//set value of parameters
                            cmd.ExecuteNonQuery(); //execute sql statement
                            cmd.Parameters["@StartStatCd"].Value = "NS4";//set value of parameters
                            cmd.Parameters["@EndStatCd"].Value = "NS5";//set value of parameters
                            cmd.Parameters["@CardFare"].Value = 0.83;//set value of parameters
                            cmd.Parameters["@TicketFare"].Value = 1.50;//set value of parameters
                            cmd.Parameters["@JourneyDuration"].Value = 6;//set value of parameters
                            cmd.ExecuteNonQuery(); //execute sql statement
                            cmd.Parameters["@StartStatCd"].Value = "NS5";//set value of parameters
                            cmd.Parameters["@EndStatCd"].Value = "NS4";//set value of parameters
                            cmd.ExecuteNonQuery(); //execute sql statement
                        }
                    }
                    catch (Exception ex) //catch exception error
                    {
                        Console.WriteLine("Unable to Insert Fare Infomation\nError Details:{0}", ex);
                    }
                    finally
                    {
                        connection.Close(); //close connection
                        Console.WriteLine("Connection closed");
                        Console.WriteLine("Records inserted: {0}", counter);
                    }
                }

            }
        }
    }
}
