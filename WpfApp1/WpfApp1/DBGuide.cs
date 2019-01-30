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
        //private const string connectionString = "Data Source=DIT-NB1828823\\SQLEXPRESS; database=APPDCADB; integrated security = true;";
        private const string connectionString = "Data Source=DIT-NB1829233\\SQLEXPRESS; database=APPDCADB; integrated security = true;";

        public static List<Line> RetrieveMRTDataFromDBtoList()
        {
            List<Line> MRT = new List<Line>();
            DataTable LineCdRef = new DataTable();

            using (SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        try
                        {
                            connection.ConnectionString = connectionString;
                            connection.Open();
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT Line_Code FROM LineCdRef";

                            da.SelectCommand = cmd;
                            da.Fill(LineCdRef);


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            //System.Windows.MessageBox.Show("Error Retriving Line Information");
                        }
                        finally
                        {
                            connection.Close();
                            Console.WriteLine("Connection closed");
                        }
                    }
                }
            }

            using (SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Parameters.Add("@LineCd", SqlDbType.Char, 2);
                    for (int LineNo = 0; LineNo < LineCdRef.Rows.Count; LineNo++)
                    {
                        try
                        {
                            connection.ConnectionString = connectionString;
                            connection.Open();
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT Line_Code, Station_Code, Station_Name FROM Station WHERE Line_Code=@LineCd";


                            DataRow LineCdRow = LineCdRef.Rows[LineNo];
                            cmd.Parameters["@LineCd"].Value = LineCdRow["Line_Code"];

                            MRT.Add(new Line(LineCdRow["Line_Code"].ToString()));

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {

                                    while (reader.Read())
                                    {
                                        MRT[LineNo].AddStationToLine(reader.GetString(1), reader.GetString(2));

                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        finally
                        {
                            connection.Close();
                            Console.WriteLine("Connection closed");

                        }
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
            return MRT;

        }

        public static List<string> QueryFareFromDatabase(string startStatCd, string endStatCd)
        {
            List<string> queryResult = new List<string>();
            using (SqlConnection connection = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT Card_Fare,Ticket_Fare,Journey_Duration FROM Fare WHERE Start_Station_Code = @StartStatCd AND End_Station_Code = @EndStatCd";
                        cmd.Parameters.Add("@StartStatCd", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@EndStatCd", SqlDbType.VarChar, 4);
                        cmd.Parameters["@StartStatCd"].Value = startStatCd;
                        cmd.Parameters["@EndStatCd"].Value = endStatCd;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        queryResult.Add(reader.GetSqlValue(i).ToString());
                                    }

                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    finally
                    {
                        connection.Close();
                        Console.WriteLine("Connection closed");

                    }
                }


            }
            return queryResult;

        }
    }
}
