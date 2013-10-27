using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

/////////////////////////////
//
//    Example of comment      
//
/////////////////////////////
//
//
//      FUN SaveLog
//****************************
//  INPUT
//      SocketOperationLog OBJECT
//  OUTPUT
//      VOID
//  FUNCIONALITY
//      Saving log to data base.
//  ERRORS
//      ERR_001
//      ERR_002

/*
 * 
 * 
 * 
 * 
 * 
 * 
 */

namespace Service
{
    class DBConnector
    {
        public MySqlConnection CreateConn()
        {
            string connectionStr = "Server=localhost;Database=wfrp_database;Uid=root;Pwd=root";

            MySqlConnection connection = new MySqlConnection(connectionStr);


            return connection;
        }


    }
}

/*EXAMPLES ONLY!!! FOR CONSTRUCTION. THERE ARE SOME MISSING EXCEPTION BLOCKS.
 * 
 * 
 //////////////////////////////////////////////
 //
 //             INSERT
 //
 //////////////////////////////////////////////
 
MySqlConnection connection = CreateConn();
            try
            {
                connection.Open();
            }
            catch (MySqlException o)
            {
                Console.WriteLine(o.ToString());
                connection.Close();
            }

            try
            {
                
                MySqlCommand cmd = connection.CreateCommand();

                cmd.CommandText = "INSERT INTO log_table(user_ip, command, date_time) VALUES(@user_ip, @command, @date_time)";
                cmd.Parameters.AddWithValue("@user_ip", SOLObj.GetClientIP().Address);
                cmd.Parameters.AddWithValue("@command", SOLObj.GetOperation().ToString());
                cmd.Parameters.AddWithValue("@date_time", SOLObj.GetDate().ToString());
                cmd.ExecuteNonQuery();

               // cmd.CommandText = "SELECT * FROM account_table WHERE acc_name = @data";
               // cmd.Parameters.AddWithValue("@data", data[0]);
               //DataSet ds = new DataSet();
               //MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
               //adap.Fill(ds);
               //Console.WriteLine(ds.Tables[0].Rows[0][3].ToString());
               //id_log = Int32.Parse(ds.Tables[0].Rows[0][0].ToString());
               //name = ds.Tables[0].Rows[0][1].ToString();
                Console.WriteLine("Log Saved");
                connection.Close();
            }
            catch(MySqlException o)
            {
                Console.WriteLine(o.ToString());
                connection.Close();
            }
  
 //////////////////////////////////////////////
 //
 //             UPADTE
 //
 //////////////////////////////////////////////
 
 try
            {
                connection.Open();
            }
            catch (MySqlException o)
            {
                Console.WriteLine(o.ToString());
            }

            try
            {
                cmd = connection.CreateCommand();

                cmd.CommandText = "UPDATE fk_table SET fk_hero_skills_table = @id_acc WHERE id = @fk_id ";
                cmd.Parameters.AddWithValue("@id_acc", data[0]);
                cmd.Parameters.AddWithValue("@fk_id", id_fk_table);

                cmd.ExecuteNonQuery();

                
            }
            catch (Exception h)
            {
                Console.WriteLine(h);
                response += "Error_025";
                
            }
  
 //////////////////////////////////////////////
 //
 //             SELECT
 //
 //////////////////////////////////////////////
  
 
 try
                {
                    connection.Open();
                }
                catch (MySqlException o)
                {
                    Console.WriteLine(o.ToString());
                }

                try
                {
                    MySqlCommand cmd = connection.CreateCommand();
                    DataSet ds = new DataSet();

                    cmd.CommandText = "SELECT id FROM account_table WHERE acc_mail = @date";
                    cmd.Parameters.AddWithValue("@date", data[2]);

                    MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                    
                    adap.Fill(ds);
                    id_log = Int32.Parse(ds.Tables[0].Rows[0][0].ToString());
                    
                    response = "Error_002";
                    Console.WriteLine(response);

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();

                    }
                }
                catch (Exception)
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();

                    }
                }
 

*/