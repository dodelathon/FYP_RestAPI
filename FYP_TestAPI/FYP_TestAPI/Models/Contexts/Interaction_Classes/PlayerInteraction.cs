using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using CS4227_Database_API.Models.Containers;
using CS4227_Database_API.Factories.Abstract_Factories;

namespace CS4227_Database_API.Models.DBContexts.Interaction_Classes
{
    public class PlayerInteraction: IInteraction
    {
        #region Class_Elements
        private MySqlConnection conn;
        private IAbFactory factory;
        private List<IDBContainer> list;

        public PlayerInteraction(MySqlConnection newConn, IAbFactory newFactory)
        {
            conn = newConn;
            factory = newFactory;
        }

        public PlayerInteraction(MySqlConnection newConn, IAbFactory newFactory, List<IDBContainer> newList)
        {
            conn = newConn;
            factory = newFactory;
            list = newList;
        }
        #endregion

        #region Getters
        public void GetAll()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from Player_Stats;", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        List<string> _params = new List<string>
                        {
                            reader["PlayerName"].ToString(),
                            reader["PlayerStatsID"].ToString(),
                            reader["RecentScore"].ToString(),
                            reader["RecentTime"].ToString()
                        };
                        list.Add((PlayerObject)factory.GetDBContainer(_params));
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void GetMultipleEntries(List<IDBContainer> Values)
        {
            try
            {
                foreach (IDBContainer Value in Values)
                {
                    PlayerObject temp = (PlayerObject)Value;
                    MySqlCommand cmd = new MySqlCommand("select * from Player_Stats where PlayerStatsID = @PID;", conn);
                    cmd.Parameters.AddWithValue("PID", temp.PlayerID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            List<string> _params = new List<string>
                            {
                                reader["PlayerName"].ToString(),
                                reader["PlayerStatsID"].ToString(),
                                reader["RecentScore"].ToString(),
                                reader["RecentTime"].ToString()
                            };
                            list.Add((PlayerObject)factory.GetDBContainer(_params));
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void GetPlayerEntry(IDBContainer Value)
        {
            try
            {
                PlayerObject temp = (PlayerObject)Value;
                MySqlCommand cmd = new MySqlCommand("select * from Player_Stats where PlayerStatsID = @PID;", conn);
                cmd.Parameters.AddWithValue("PID", temp.PlayerID);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        List<string> _params = new List<string>
                        {
                            reader["PlayerName"].ToString(),
                            reader["PlayerStatsID"].ToString(),
                            reader["RecentScore"].ToString(),
                            reader["RecentTime"].ToString()
                        };
                        list.Add((PlayerObject)factory.GetDBContainer(_params));
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Broke here");
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region Adders
        public void AddEntry(IDBContainer Value)
        {
            try
            {
                if (!Exists(Value))
                {
                    PlayerObject temp = (PlayerObject)Value;
                    MySqlCommand cmd = new MySqlCommand("insert into Player_Stats values(Default, @ID, @Name, @RecentScore, @RecentTime);", conn);
                    cmd.Parameters.AddWithValue("@ID", temp.PlayerID);
                    cmd.Parameters.AddWithValue("@Name", temp.PlayerName);
                    cmd.Parameters.AddWithValue("@RecentScore", temp.RecentScore);
                    cmd.Parameters.AddWithValue("@RecentTime", temp.RecentTime);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    throw new Exception("Entry Already Exists");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void AddEntry(List<IDBContainer> Values)
        {
            try
            {
                foreach (IDBContainer Value in Values)
                {
                    if (!Exists(Value))
                    {
                        PlayerObject temp = (PlayerObject)Value;
                        MySqlCommand cmd = new MySqlCommand("insert into Player_Stats values(Default, @ID, @Name, @RecentScore, @RecentTime);", conn);
                        cmd.Parameters.AddWithValue("@ID", temp.PlayerID);
                        cmd.Parameters.AddWithValue("@Name", temp.PlayerName);
                        cmd.Parameters.AddWithValue("@RecentScore", temp.RecentScore);
                        cmd.Parameters.AddWithValue("@RecentTime", temp.RecentTime);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        throw new Exception("One or more Entries already Exist");
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private bool Exists(IDBContainer Value)
        {
            PlayerObject temp = (PlayerObject)Value;
            MySqlCommand cmd = new MySqlCommand("select * from Player_Stats where PlayerName = @name;", conn);
            List<IDBContainer> containers = new List<IDBContainer>();
            cmd.Parameters.AddWithValue("@name", temp.PlayerName);
            cmd.Prepare();


            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    List<string> _params = new List<string>
                        {
                            reader["PlayerName"].ToString(),
                            reader["PlayerStatsID"].ToString(),
                            reader["RecentScore"].ToString(),
                            reader["RecentTime"].ToString()
                        };
                    containers.Add((PlayerObject)factory.GetDBContainer(_params));
                }
            }

            if (containers.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Updaters
        public void UpdateEntry(IDBContainer Value)
        {
            try
            {
                PlayerObject temp = (PlayerObject)Value;
                MySqlCommand cmd = new MySqlCommand("Update Player_Stats Set PlayerName = @name, RecentScore = @RecentScore, RecentTime = @RecentTime where PlayerStatsID = @ID;", conn);
                cmd.Parameters.AddWithValue("@ID", temp.PlayerID);
                cmd.Parameters.AddWithValue("@Name", temp.PlayerName);
                cmd.Parameters.AddWithValue("@RecentScore", temp.RecentScore);
                cmd.Parameters.AddWithValue("@RecentTime", temp.RecentTime);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void UpdateEntry(List<IDBContainer> Values)
        {
            try
            {
                foreach (IDBContainer Value in Values)
                {
                    PlayerObject temp = (PlayerObject)Value;
                    MySqlCommand cmd = new MySqlCommand("Update Player_Stats Set PlayerName = @name, RecentScore = @RecentScore, RecentTime = @RecentTime where PlayerStatsID = @ID;", conn);
                    cmd.Parameters.AddWithValue("@ID", temp.PlayerID);
                    cmd.Parameters.AddWithValue("@Name", temp.PlayerName);
                    cmd.Parameters.AddWithValue("@RecentScore", temp.RecentScore);
                    cmd.Parameters.AddWithValue("@RecentTime", temp.RecentTime);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region Deleters
        public void DeleteEntry(IDBContainer Value)
        {
            try
            {
                PlayerObject temp = (PlayerObject)Value;
                MySqlCommand cmd = new MySqlCommand("Delete from Player_Stats where PlayerStatsID = @ID;", conn);
                cmd.Parameters.AddWithValue("@ID", temp.PlayerID);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void DeleteEntry(List<IDBContainer> Values)
        {
            try
            {
                foreach (IDBContainer Value in Values)
                {
                    PlayerObject temp = (PlayerObject)Value;
                    MySqlCommand cmd = new MySqlCommand("Delete from Player_Stats where PlayerStatsID = @ID;", conn);
                    cmd.Parameters.AddWithValue("@ID", temp.PlayerID);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion
    }
}
    

