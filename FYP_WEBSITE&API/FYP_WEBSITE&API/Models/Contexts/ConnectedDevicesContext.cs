using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using FYP_TestAPI.Models.Containers;

namespace FYP_TestAPI.Models.Contexts
{   
    /* This class describes the interactions available with the Database
    */
    public class ConnectedDevicesContext
    {

        //Enum to describe the type of interaction to perform
        public enum DatabaseGetMode
        {
            UUID,
            Name
        }

        public string ConnectionString { get; set; }

        //Constructor to set connection string. 
        public ConnectedDevicesContext(string cString)
        {
            ConnectionString = cString;
        }

        //Creates a MySQL connection and returns it.
        private MySqlConnection GetConnection()
        {
            try
            {
                return new MySqlConnection(ConnectionString);
            }
            catch
            {
                return null;
            }
        }

        //Returns a list of all the all the devices in the database.
        public List<FeederDevice> GetAllDevices()
        {
            List<FeederDevice> list = null;

            using (MySqlConnection conn = GetConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        list = new List<FeederDevice>();
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("select * from Connected_Devices", conn);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new FeederDevice()
                                {
                                    DeviceID = Convert.ToInt32(reader["DeviceID"]),
                                    Device_Name = reader["Device_Name"].ToString(),
                                    UUID = reader["UUID"].ToString()
                                });
                            }
                        }
                        conn.Close();
                    }
                    catch
                    {
                        list = null;
                    }
                }
            }
            return list;
        }

        //Uses the enum to find and return a device by searching via UUID or Name
        public FeederDevice GetDevice(string Device_Val, DatabaseGetMode mode)
        {
            FeederDevice retval = null;
            using (MySqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = null;
                    if (mode == DatabaseGetMode.UUID)
                    {
                        cmd = new MySqlCommand("select * from Connected_Devices where UUID = @_UUID;", conn);
                        cmd.Parameters.AddWithValue("_UUID", Device_Val);
                    }
                    else if(mode == DatabaseGetMode.Name)
                    {
                        cmd = new MySqlCommand("select * from Connected_Devices where Device_Name = @Name;", conn);
                        cmd.Parameters.AddWithValue("Name", Device_Val);
                    }
                    cmd.Prepare();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retval = new FeederDevice
                            {
                                DeviceID = Convert.ToInt32(reader["DeviceID"]),
                                Device_Name = reader["Device_Name"].ToString(),
                                UUID = reader["UUID"].ToString()
                            };
                        }
                    }
                    conn.Close();
                }
                catch
                {
                    retval = null;
                    Console.WriteLine("Error when searching");
                }
            }
            return retval;
        }

        //Adds a device to the database, requires both a Name and UUID
        public bool AddDevice(string DevName, string DevUUID)
        {
            bool complete = false;
            FeederDevice temp = GetDevice(DevName, DatabaseGetMode.Name);
            if (temp == null)
            {
                using (MySqlConnection conn = GetConnection())
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO Connected_Devices VALUES(default, @_Name, @_UUID);", conn);
                        cmd.Parameters.AddWithValue("_UUID", DevUUID);
                        cmd.Parameters.AddWithValue("_Name", DevName);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        complete = true;
                    }
                    catch
                    {
                        complete = false;
                    }
                }
            }
            return complete;
        }

        //Removes a device from the database via UUID
        public bool RemoveDevice(string DevUUID)
        {
            bool complete = false;
            FeederDevice temp = GetDevice(DevUUID, DatabaseGetMode.UUID);
            if (temp != null)
            {
                using (MySqlConnection conn = GetConnection())
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("Delete from Connected_Devices where UUID=@_UUID;", conn);
                        cmd.Parameters.AddWithValue("_UUID", DevUUID);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        complete = true;
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message + "\n" + e.StackTrace);
                        complete = false;
                    }
                }
            }
            else
            {
                complete = true;
            }
            return complete;
        }

        //Wrapper method for GetDevice, returns bool depending on if a device is found.
        public bool Exists(string Device_Val, DatabaseGetMode mode)
        {
            FeederDevice res = GetDevice(Device_Val, mode);
            if (res == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
