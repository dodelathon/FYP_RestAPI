using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using FYP_TestAPI.Models.Containers;

namespace FYP_TestAPI.Models.Contexts
{
    public class ConnectedDevicesContext
    {
        public string ConnectionString { get; set; }

        public ConnectedDevicesContext(string cString)
        {
            ConnectionString = cString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<FeederDevice> GetAllDevices()
        {
            List<FeederDevice> list = new List<FeederDevice>();

            using (MySqlConnection conn = GetConnection())
            {
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
                        });
                    }
                }
                conn.Close();
            }
            return list;
        }
    }
}
