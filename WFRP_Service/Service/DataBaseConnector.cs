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

        public string GetHeroEyes(string race)
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

        public List<string> GetOccupationsByRace(string race)
        {
            List<string> response = new List<string>();
            MySqlConnection connection = CreateConn();
            int race_choise = 0;
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
                
                cmd.CommandText = "SELECT * FROM occupation_table";
                
                MySqlDataAdapter adap =  new MySqlDataAdapter(cmd);

                if(race == "człowiek") race_choise = 1;
                else if(race == "elf") race_choise = 2;
                else if(race == "krasnolud") race_choise = 3;
                else if(race == "niziołek") race_choise = 4;

                if (race_choise == 1)
                {
                    cmd.CommandText = "SELECT name FROM occupation_table WHERE human = @true";
                    cmd.Parameters.AddWithValue("@true", 1);

                    adap = new MySqlDataAdapter(cmd);

                    adap.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        response.Add(ds.Tables[0].Rows[i][0].ToString());
                    }

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();

                    }
                }
                else if (race_choise == 2)
                {
                    cmd.CommandText = "SELECT name FROM occupation_table WHERE elf = @true";
                    cmd.Parameters.AddWithValue("@true", 1);

                    adap = new MySqlDataAdapter(cmd);

                    adap.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        response.Add(ds.Tables[0].Rows[i][0].ToString());
                    }

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();

                    }
                }
                else if (race_choise == 3)
                {
                    cmd.CommandText = "SELECT name FROM occupation_table WHERE dwarf = @true";
                    cmd.Parameters.AddWithValue("@true", 1);

                    adap = new MySqlDataAdapter(cmd);

                    adap.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        response.Add(ds.Tables[0].Rows[i][0].ToString());
                    }

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();

                    }
                }
                else if (race_choise == 4)
                {
                    cmd.CommandText = "SELECT name FROM occupation_table WHERE halfling = @true";
                    cmd.Parameters.AddWithValue("@true", 1);

                    adap = new MySqlDataAdapter(cmd);

                    adap.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        response.Add(ds.Tables[0].Rows[i][0].ToString());
                    }

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();

                    }
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

        public string GetOccupationInfo(string occupation)
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

                cmd.CommandText = "SELECT info FROM occupation_table WHERE name = @name";
                cmd.Parameters.AddWithValue("@name", occupation);

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

        public KeyValuePair<bool, string> CreateHeroPartBasicInfo(HeroBasicInfo info)
        {
            KeyValuePair<bool, string> result = new KeyValuePair<bool, string>();
            MySqlConnection connection = CreateConn();
            int race = 0;
            int basicInfoTableID = 0;

            if (info.Race == "człowiek") race = 1;
            else if (info.Race == "elf") race = 2;
            else if (info.Race == "krasnolud") race = 3;
            else if (info.Race == "niziołek") race = 4;
            
            try
                {
                    connection.Open();
                }
                catch (MySqlException o)
                {
                    Console.WriteLine(o.ToString());
                }
            
            MySqlCommand cmd = connection.CreateCommand();
            
            //Create FK_TABLE for USER
            try
            {
                    cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO fk_table(id) VALUES(@id_acc)";
                    cmd.Parameters.AddWithValue("@id_acc", info.AccountID);
                    cmd.ExecuteNonQuery();

           //Combo Insert Hero Basic Info
                    cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO hero_base_info_table(hero_name,hero_history,hero_race,hero_sex,hero_friends,"
                        + "hero_family,hero_height,hero_weight,age,eye_color,origin,social_position,HHWB,why_travel,enemies,"
                        + "what_like,what_dont_like,personality,motivation,WHS) VALUES(@hero_nameI,@hero_historyI,@hero_raceI,"
                        + "@hero_sexI,@hero_friendsI,@hero_familyI,@hero_heightI,@hero_weightI,@ageI,@eye_colorI,@originI,@social_positionI,@HHWBI,"
                        + "@why_travelI,@enemiesI,@what_likeI,@what_dont_likeI,@personalityI,@motivationI,@WHSI)";
                    
                    cmd.Parameters.AddWithValue("@hero_nameI", info.HeroName);
                    cmd.Parameters.AddWithValue("@hero_historyI", info.AccountID);
                    cmd.Parameters.AddWithValue("@hero_raceI", race);
                    cmd.Parameters.AddWithValue("@hero_sexI", info.Sex);
                    cmd.Parameters.AddWithValue("@hero_friendsI",info.Friends);
                    cmd.Parameters.AddWithValue("@hero_familyI",info.Family);
                    cmd.Parameters.AddWithValue("@hero_heightI",info.Height);
                    cmd.Parameters.AddWithValue("@hero_weightI",info.Weight);
                    cmd.Parameters.AddWithValue("@ageI",info.Age);
                    cmd.Parameters.AddWithValue("@eye_colorI",info.EyeColor);
                    cmd.Parameters.AddWithValue("@originI",info.Origin);
                    cmd.Parameters.AddWithValue("@social_positionI",info.SocialPosition );
                    cmd.Parameters.AddWithValue("@HHWBI",info.HHWB);
                    cmd.Parameters.AddWithValue("@why_travelI", info.WhyTravel);
                    cmd.Parameters.AddWithValue("@enemiesI", info.Enemies);
                    cmd.Parameters.AddWithValue("@what_likeI", info.Likes);
                    cmd.Parameters.AddWithValue("@what_dont_likeI", info.DontLikes);
                    cmd.Parameters.AddWithValue("@personalityI", info.Personality);
                    cmd.Parameters.AddWithValue("@motivationI", info.Motivation);
                    cmd.Parameters.AddWithValue("@WHSI", info.WhoHeServes);
                    cmd.ExecuteNonQuery();
                    
                    cmd = connection.CreateCommand();
                    DataSet ds = new DataSet();

                    cmd.CommandText = "SELECT id FROM hero_base_info_table WHERE hero_history = @id_accI";
                    cmd.Parameters.AddWithValue("@id_accI", info.AccountID);

                    MySqlDataAdapter adap = new MySqlDataAdapter(cmd);

                    adap.Fill(ds);
                    basicInfoTableID = Int32.Parse(ds.Tables[0].Rows[0][0].ToString());

                    cmd = connection.CreateCommand();
                    cmd.CommandText = "UPDATE fk_table SET fk_hero_base_info_table = @HBITID WHERE id = @fk_id ";
                    cmd.Parameters.AddWithValue("@fk_id", info.AccountID);
                    cmd.Parameters.AddWithValue("@HBITID", basicInfoTableID);
                    cmd.ExecuteNonQuery();
                    
                    result = new KeyValuePair<bool, string>(true, "Success");

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();

                    }
            }
            catch (MySqlException o)
            {
                Console.WriteLine(o.ToString());
            }

            
            return result;
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