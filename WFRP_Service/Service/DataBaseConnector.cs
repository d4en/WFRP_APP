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
    public class DBConnector
    {
        public MySqlConnection CreateConn()
        {
            string connectionStr = "Server=localhost;Database=wfrp_database;Uid=root;Pwd=root";

            MySqlConnection connection = new MySqlConnection(connectionStr);


            return connection;
        }

        public KeyValuePair<bool, string> Register(Client client)
        {
            MySqlConnection connection = CreateConn();
            string acc_name = client.Name;
            string acc_pas = client.Password;
            KeyValuePair<bool, string> result = new KeyValuePair<bool, string>();

            Console.WriteLine(client.Name);
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

                    cmd.CommandText = "SELECT id FROM account_table WHERE acc_name = @name";
                    cmd.Parameters.AddWithValue("@name", client.Name);

                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        result = new KeyValuePair<bool, string>(false, "Name Already Exists");
                        connection.Close();
                    }
                    else
                    {
                        connection.Close();
                        connection.Open();
                        cmd = connection.CreateCommand();
                        cmd.CommandText = "INSERT INTO account_table(acc_name, acc_pas) VALUES(@in_name,@in_pas)";
                        cmd.Parameters.AddWithValue("@in_name", client.Name);
                        cmd.Parameters.AddWithValue("@in_pas", client.Password);
                        cmd.ExecuteNonQuery();
                        result = new KeyValuePair<bool,string>(true, "Registered");
                    }
                    
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();

                    }
                }
                catch (Exception w)
                {
                    Console.WriteLine(w.ToString());
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();

                    }
                }
                return result;
        }

        public KeyValuePair<bool, string> LogIn(Client client)
        {
            MySqlConnection connection = CreateConn();
            int id_log = 0;
            KeyValuePair<bool, string> result = new KeyValuePair<bool, string>();
            string acc_name = client.Name;
            string acc_pas = client.Password;

            Console.WriteLine(client.Name);
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

                cmd.CommandText = "SELECT * FROM account_table WHERE acc_name = @name";
                cmd.Parameters.AddWithValue("@name", client.Name);
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);

                adap.Fill(ds);
                id_log = Int32.Parse(ds.Tables[0].Rows[0][0].ToString());
                acc_name = ds.Tables[0].Rows[0][1].ToString();
                acc_pas = ds.Tables[0].Rows[0][2].ToString();

                if (acc_pas == client.Password)
                {
                    result = new KeyValuePair<bool,string> (true, ds.Tables[0].Rows[0][0].ToString());
                }
                else
                {
                    result = new KeyValuePair<bool, string> (false, "Wrong credentials");
                }

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                result = new KeyValuePair<bool, string>(false, ex.ToString());
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();

                }
            }

            return result;

        }

        public string GerHeroEyes(string race)
        {
            string response = string.Empty;
            MySqlConnection connection = CreateConn();
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

                cmd.CommandText = "SELECT color FROM custom_eyes WHERE race = @race";
                cmd.Parameters.AddWithValue("@race", race);

                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);

                adap.Fill(ds);
                response = ds.Tables[0].Rows[0][0].ToString();

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
            return response;
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