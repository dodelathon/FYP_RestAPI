using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS4227_Database_API.Models.Containers;
using CS4227_Database_API.Factories.Abstract_Factories;
using MySql.Data.MySqlClient;

namespace CS4227_Database_API.Models.DBContexts.Interaction_Classes
{
    public class LeaderboardInteraction: IInteraction
    {
        #region Class_Elements
        private MySqlConnection conn;
        private IAbFactory factory;
        private List<IDBContainer> list;

        public LeaderboardInteraction(MySqlConnection newConn, IAbFactory newFactory)
        {
            conn = newConn;
            factory = newFactory;
        }

        public LeaderboardInteraction(MySqlConnection newConn, IAbFactory newFactory, List<IDBContainer> newList)
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
                MySqlCommand cmd = new MySqlCommand("select * from Leaderboard;", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        List<string> _params = new List<string>
                        {
                            reader["PlayerName"].ToString(),
                            reader["PlayerLeaderID"].ToString(),
                            reader["HighScore"].ToString(),
                            reader["ScoreTime"].ToString()
                        };
                        list.Add((LeaderboardObject)factory.GetDBContainer(_params));
                    }
                }
            }
            catch(Exception e)
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
                    LeaderboardObject temp = (LeaderboardObject)Value;
                    MySqlCommand cmd = new MySqlCommand("select * from Leaderboard where PlayerLeaderID = @PID;", conn);
                    cmd.Parameters.AddWithValue("PID", temp.PlayerID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            List<string> _params = new List<string>
                            {
                                reader["PlayerName"].ToString(),
                                reader["PlayerLeaderID"].ToString(),
                                reader["HighScore"].ToString(),
                                reader["ScoreTime"].ToString()
                            };
                            list.Add((LeaderboardObject)factory.GetDBContainer(_params));
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
                LeaderboardObject temp = (LeaderboardObject)Value;
                MySqlCommand cmd = new MySqlCommand("select * from Leaderboard where PlayerLeaderID = @PID;", conn);
                cmd.Parameters.AddWithValue("PID", temp.PlayerID);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        List<string> _params = new List<string>
                        {
                            reader["PlayerName"].ToString(),
                            reader["PlayerLeaderID"].ToString(),
                            reader["HighScore"].ToString(),
                            reader["ScoreTime"].ToString()
                        };
                        list.Add((LeaderboardObject)factory.GetDBContainer(_params));
                    }
                }

            }
            catch (Exception e)
            {
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
                    LeaderboardObject temp = (LeaderboardObject)Value;
                    MySqlCommand cmd = new MySqlCommand("insert into Leaderboard values(default, @ID, @Name, @Highscore, @Time);", conn);
                    cmd.Parameters.AddWithValue("@ID", temp.PlayerID);
                    cmd.Parameters.AddWithValue("@Name", temp.PlayerName);
                    cmd.Parameters.AddWithValue("@Highscore", temp.Highscore);
                    cmd.Parameters.AddWithValue("@Time", temp.ScoreTime);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    throw new Exception("Entry Already Exists");
                }
            }
            catch(Exception e)
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
                        LeaderboardObject temp = (LeaderboardObject)Value;
                        MySqlCommand cmd = new MySqlCommand("insert into Leaderboard values(default, @ID, @Name, @Highscore, @Time);", conn);
                        cmd.Parameters.AddWithValue("@ID", temp.PlayerID);
                        cmd.Parameters.AddWithValue("@Name", temp.PlayerName);
                        cmd.Parameters.AddWithValue("@Highscore", temp.Highscore);
                        cmd.Parameters.AddWithValue("@Time", temp.ScoreTime);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        throw new Exception("One or more entries already Exist");
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
            LeaderboardObject temp = (LeaderboardObject)Value;
            MySqlCommand cmd = new MySqlCommand("select * from Leaderboard where PlayerName = @name;", conn);
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
                            reader["PlayerLeaderID"].ToString(),
                            reader["HighScore"].ToString(),
                            reader["ScoreTime"].ToString()
                        };
                    containers.Add((LeaderboardObject)factory.GetDBContainer(_params));
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
                LeaderboardObject temp = (LeaderboardObject)Value;
                MySqlCommand cmd = new MySqlCommand("Update Leaderboard Set HighScore = @score, ScoreTime = @time where PlayerLeaderID = @ID;", conn);
                cmd.Parameters.AddWithValue("@ID", temp.PlayerID);
                cmd.Parameters.AddWithValue("@score", temp.Highscore);
                cmd.Parameters.AddWithValue("@time", temp.ScoreTime);
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
                    LeaderboardObject temp = (LeaderboardObject)Value;
                    MySqlCommand cmd = new MySqlCommand("Update Leaderboard Set HighScore = @score, ScoreTime = @time where PlayerLeaderID = @ID;", conn);
                    cmd.Parameters.AddWithValue("@ID", temp.PlayerID);
                    cmd.Parameters.AddWithValue("@score", temp.Highscore);
                    cmd.Parameters.AddWithValue("@time", temp.ScoreTime);
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
                LeaderboardObject temp = (LeaderboardObject)Value;
                MySqlCommand cmd = new MySqlCommand("Delete from Leaderboard where PlayerLeaderID = @ID;", conn);
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
                    LeaderboardObject temp = (LeaderboardObject)Value;
                    MySqlCommand cmd = new MySqlCommand("Delete from Leaderboard where PlayerLeaderID = @ID;", conn);
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
