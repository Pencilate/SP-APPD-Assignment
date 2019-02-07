using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace WpfApp1
{
    class DBGuide
    {
        //Make sure to check the connection string
        private const string connectionString = "Data Source=DIT-NB1828823\\SQLEXPRESS; database=APPDCADB; integrated security = true;";
        //private const string connectionString = "Data Source=DIT-NB1829233\\SQLEXPRESS; database=APPDCADB; integrated security = true;";

        public static List<Line> RetrieveMRTDataFromDBtoList() //Retrieves MRT Data from Database and stores it in a list
        {
            List<Line> MRT = new List<Line>(); //new list
            DataTable LineCdRef = new DataTable(); //new datatable

            using (SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        try
                        {
                            connection.ConnectionString = connectionString; //sets connection string
                            connection.Open(); //opens connection
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT Line_Code FROM LineCdRef"; //sql statement

                            da.SelectCommand = cmd;
                            da.Fill(LineCdRef); //fill dataadapter with LineCd


                        }
                        catch (Exception ex) //catches errors
                        {
                            Console.WriteLine(ex);
                            //System.Windows.MessageBox.Show("Error Retriving Line Information");
                        }
                        finally
                        {
                            connection.Close(); //close connection
                            Console.WriteLine("Connection closed");
                        }
                    }
                }
            }

            using (SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Parameters.Add("@LineCd", SqlDbType.Char, 2); //add sql parameters
                    for (int LineNo = 0; LineNo < LineCdRef.Rows.Count; LineNo++) //for loop
                    {
                        try
                        {
                            connection.ConnectionString = connectionString; //sets connection string
                            connection.Open(); //open connection
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT Line_Code, Station_Code, Station_Name FROM Station WHERE Line_Code=@LineCd ORDER BY Id ASC"; //sql statement


                            DataRow LineCdRow = LineCdRef.Rows[LineNo]; //DataRow 
                            cmd.Parameters["@LineCd"].Value = LineCdRow["Line_Code"]; //sets value of parameter

                            MRT.Add(new Line(LineCdRow["Line_Code"].ToString())); //converts LineCode to string and adds it to MRT List

                            using (SqlDataReader reader = cmd.ExecuteReader()) //execute command
                            {
                                if (reader.HasRows)
                                {

                                    while (reader.Read())
                                    {
                                        MRT[LineNo].AddStationToLine(reader.GetString(1), reader.GetString(2)); //add station to the line

                                    }
                                }
                            }

                        }
                        catch (Exception ex) //catch exception
                        {
                            Console.WriteLine(ex);
                        }
                        finally
                        {
                            connection.Close(); //close connection
                            Console.WriteLine("Connection closed");

                        }
                    }
                }
            }

            //Comparing station to set them as interchange
            for (int count = (MRT.Count - 1); count > 0; count--) //for loop
            {
                int CurrentLine = count;
                int ComparedLine = count - 1;
                for (int i = ComparedLine; i >= 0; i--) //for loop
                {

                    for (int j = 0; j < MRT[CurrentLine].StationList.Count(); j++) //for loop
                    {

                        for (int h = 0; h < MRT[i].StationList.Count(); h++) //for loop
                        {
                            //Compare name of stations between CurruntLine and ComparedLine
                            Console.WriteLine("{0} {1} VS {2} {3}", MRT[CurrentLine].LineCd, MRT[CurrentLine].StationList[j].StationName, MRT[i].LineCd, MRT[i].StationList[h].StationName);
                            if (MRT[CurrentLine].StationList[j].StationName.Equals(MRT[i].StationList[h].StationName)) //if station names match
                            {                                                         
                                MRT[CurrentLine].StationList[j].IsInterchange = true;
                                //set varible to identify it as a interchange as true


                                Console.WriteLine("YES");
                                foreach (string str in MRT[CurrentLine].StationList[j].StationCode) //foreach loop
                                {
                                    Console.Write(str + " ");
                                }
                                Console.WriteLine();
                                foreach (string str in MRT[i].StationList[h].StationCode) //foreach loop
                                {
                                    Console.Write(str + " ");
                                }
                                Console.WriteLine();

                                if ((MRT[CurrentLine].StationList[j].StationCode) != (MRT[i].StationList[h].StationCode)) //if
                                {
                                    MRT[CurrentLine].StationList[j].StationCode.AddRange(MRT[i].StationList[h].StationCode);
                                }

                                foreach (string str in MRT[CurrentLine].StationList[j].StationCode) //foreach loop
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
            return MRT; //return MRT List

        }

        public static List<string> QueryFareFromDatabase(string startStatCd, string endStatCd) //Returns Trip info after querying database
        {
            List<string> queryResult = new List<string>(); //new List
            using (SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString; //sets connectionstring
                        connection.Open(); //open connection
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT Card_Fare,Ticket_Fare,Journey_Duration FROM Fare WHERE Start_Station_Code = @StartStatCd AND End_Station_Code = @EndStatCd"; //sql command
                        cmd.Parameters.Add("@StartStatCd", SqlDbType.VarChar, 4); //add sql parameters
                        cmd.Parameters.Add("@EndStatCd", SqlDbType.VarChar, 4); //add sql parameters
                        cmd.Parameters["@StartStatCd"].Value = startStatCd; //set value of parameter
                        cmd.Parameters["@EndStatCd"].Value = endStatCd; //set value of parameter

                        using (SqlDataReader reader = cmd.ExecuteReader()) //execute sql command
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        queryResult.Add(reader.GetSqlValue(i).ToString()); //convert value to string and add it to List
                                    }

                                }
                            }
                        }

                        if (queryResult.Count == 0) //special case
                        {
                            queryResult.Clear();
                            queryResult.Add("10");
                            queryResult.Add("10");
                            queryResult.Add("80");
                        }

                    }
                    catch (Exception ex) //catch exception errors
                    {
                        Console.WriteLine(ex);

                    }
                    finally
                    {
                        connection.Close(); //close connection
                        Console.WriteLine("Connection closed");
                    }
                }
            }
            return queryResult; //return list
        }

        //INSERT Past Queries into DB
        public static void InsertFareDataIntoHistory(string sSc, string eSc, char type)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        List<string> queryResult = QueryFareFromDatabase(sSc, eSc); //invoke QueryFareFromDatabase method and store result in list string
                        conn.ConnectionString = connectionString; //sets connection string
                        cmd.Connection = conn;
                        conn.Open(); //open connection
                        Console.WriteLine("Connection open.");
                        cmd.CommandText = "INSERT INTO FareHistory (Start_Station_Code,End_Station_Code,Date_Queried,Fare_Type,Fare) VALUES (@StartStatCd,@EndStatCd,@Date,@FareType,@Fare)"; //sql statement

                        cmd.Parameters.Add("@StartStatCd", SqlDbType.VarChar, 4); //add sql parameters
                        cmd.Parameters.Add("@EndStatCd", SqlDbType.VarChar, 4); //add sql parameters
                        cmd.Parameters.Add("@FareType", SqlDbType.Char, 1); //add sql parameters
                        cmd.Parameters.Add("@Fare", SqlDbType.Money); //add sql parameters
                        cmd.Parameters.Add("@Date", SqlDbType.Date); //add sql parameters

                        cmd.Parameters["@StartStatCd"].Value = sSc; //set value of parameter
                        cmd.Parameters["@EndStatCd"].Value = eSc; //set value of parameter
                        cmd.Parameters["@Date"].Value = DateTime.Now.Date; //set value of parameter
                        Console.WriteLine(cmd.Parameters["@Date"].Value.ToString());
                        switch (type) //switch
                        {
                            case 'C':
                                cmd.Parameters["@Fare"].Value = double.Parse(queryResult[0]);
                                cmd.Parameters["@FareType"].Value = type;
                                break;
                            case 'T':
                                cmd.Parameters["@Fare"].Value = double.Parse(queryResult[1]);
                                cmd.Parameters["@FareType"].Value = type;
                                break;

                        }

                        cmd.ExecuteNonQuery(); //execute sql statement
                        Console.WriteLine("Record successfully added.");
                    }

                    catch (Exception ex) //catch exception
                    {
                        Console.WriteLine("Record failed to add in as it already exists.\n" + ex);

                    }

                    finally
                    {
                        conn.Close(); //close connection
                        Console.WriteLine("Connection closed.");
                    }
                }
            }
        }

        public static DataTable RetrieveFareRecordFromDatabase() //retrieve Fare Record from the database
        {
            DataTable FareHistoryRecords = new DataTable(); // new datatable

            using (SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        try
                        {
                            connection.ConnectionString = connectionString; //set connection string
                            connection.Open(); //open connection
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT F.Start_Station_Code AS 'Boarding Station Code',S1.Station_Name AS 'Boarding Station Name', F.End_Station_Code AS 'Alighting Station Code', S2.Station_Name AS 'Alighting Station Name',F.Journey_Duration AS 'Journey Duration', FH.Fare_Type AS 'Fare Type' ,FH.Fare AS 'Fare(S$)' FROM Fare AS F, FareHistory AS FH, Station AS S1, Station AS S2 WHERE F.Start_Station_Code = FH.Start_Station_Code AND F.Start_Station_Code = S1.Station_Code AND F.End_Station_Code = FH.End_Station_Code AND F.End_Station_Code = S2.Station_Code"; //sql select statement

                            da.SelectCommand = cmd;
                            da.Fill(FareHistoryRecords); //fill dataadapter with datatable


                        }
                        catch (Exception ex) //catch exception
                        {
                            Console.WriteLine(ex);
                        }
                        finally
                        {
                            connection.Close(); //close connection
                            Console.WriteLine("Connection closed");
                        }
                    }
                }
            }
            return FareHistoryRecords; //return datatable
        }
        public static DataTable RetrieveFareRecordFromDatabase(DateTime selectedDate)
        {
            DataTable FareHistoryRecords = new DataTable(); //new datatable

            using (SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        try
                        {
                            connection.ConnectionString = connectionString; //set connection string
                            connection.Open(); //open connection
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT F.Start_Station_Code AS 'Boarding Station Code',S1.Station_Name AS 'Boarding Station Name', F.End_Station_Code AS 'Alighting Station Code', S2.Station_Name AS 'Alighting Station Name',F.Journey_Duration AS 'Journey Duration', FH.Fare_Type AS 'Fare Type' ,FH.Fare AS 'Fare(S$)' FROM Fare AS F, FareHistory AS FH, Station AS S1, Station AS S2 WHERE F.Start_Station_Code = FH.Start_Station_Code AND F.Start_Station_Code = S1.Station_Code AND F.End_Station_Code = FH.End_Station_Code AND F.End_Station_Code = S2.Station_Code AND Date_Queried = @Date"; //sql select statement
                            cmd.Parameters.Add("@Date",SqlDbType.Date); //add parameters
                            cmd.Parameters["@Date"].Value = selectedDate.Date; //set value of parameter
                            da.SelectCommand = cmd;
                            da.Fill(FareHistoryRecords); //fill dataadapter with datatable


                        }
                        catch (Exception ex) //catch exception errors
                        {
                            Console.WriteLine(ex);
                        }
                        finally
                        {
                            connection.Close(); //close connection
                            Console.WriteLine("Connection closed");
                        }
                    }
                }
            }
            return FareHistoryRecords; //return DataTable
        }

        public static double FareHistoryTotalCollected() //calculate total fare collected
        {
            double totalFareCollected = 0; //total fare collected variable
            using (SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString; //sets connection string
                        connection.Open(); //open connection
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT SUM(Fare) AS 'Total Fare Collected' FROM FareHistory"; //sql select statement


                        using (SqlDataReader reader = cmd.ExecuteReader()) //execute sql statement
                        {
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {
                                    totalFareCollected = (double) reader.GetDecimal(0);

                                }
                            }
                        }

                    }
                    catch (Exception ex) //catch exception error
                    {
                        Console.WriteLine(ex);
                    }
                    finally
                    {
                        connection.Close(); //close connection
                        Console.WriteLine("Connection closed");

                    }

                }
            }
            return totalFareCollected; //return totalfare collected
        }

        public static double FareHistoryTotalCollected(DateTime selectedDate) //total fare history collected
        {
            double totalFareCollected = 0; //total fare collected variable
            using (SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString; //set connection string
                        connection.Open(); //open connection
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT SUM(Fare) AS 'Total Fare Collected' FROM FareHistory WHERE Date_Queried = @Date"; //sql select statement
                        cmd.Parameters.Add("@Date", SqlDbType.Date); //add sql parameters
                        cmd.Parameters["@Date"].Value = selectedDate.Date; //set value of sql parameters

                        using (SqlDataReader reader = cmd.ExecuteReader()) //execute sql statement
                        {
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {
                                    totalFareCollected = (double)reader.GetDecimal(0);

                                }
                            }
                        }

                    }
                    catch (Exception ex) //catch exception error
                    {
                        Console.WriteLine(ex);
                    }
                    finally
                    {
                        connection.Close(); //close connection
                        Console.WriteLine("Connection closed");

                    }

                }
            }
            return totalFareCollected; //return total fare collected
        }
    }
}


