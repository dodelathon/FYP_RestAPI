using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using CS4227_Database_API.Models.Containers;
using CS4227_Database_API.Factories.Abstract_Factories;

namespace CS4227_Database_API.Models.DBContexts.Interaction_Classes
{
    public class LoginInteraction: IInteraction
    {

        #region Class_Elements
        private MySqlConnection conn;
        private IAbFactory factory;
        private List<IDBContainer> list;

        public LoginInteraction(MySqlConnection newConn, IAbFactory newFactory)
        {
            conn = newConn;
            factory = newFactory;
        }

        public LoginInteraction(MySqlConnection newConn, IAbFactory newFactory, List<IDBContainer> newList)
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
                MySqlCommand cmd = new MySqlCommand("select * from Login_Info;", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        List<string> _params = new List<string>
                        {
                            reader["PlayerName"].ToString(),
                            reader["PlayerLogID"].ToString(),
                            reader["Password"].ToString()
                        };
                        list.Add((LoginObject)factory.GetDBContainer(_params));
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
                    LoginObject temp = (LoginObject)Value;
                    MySqlCommand cmd = new MySqlCommand("select * from Login_Info where PlayerLogID = @PID;", conn);
                    cmd.Parameters.AddWithValue("@PID", temp.PlayerID);
                    cmd.Prepare();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            List<string> _params = new List<string>
                            {
                                reader["PlayerName"].ToString(),
                                reader["PlayerLogID"].ToString(),
                                reader["Password"].ToString()
                            };
                            list.Add((LoginObject)factory.GetDBContainer(_params));
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
                LoginObject temp = (LoginObject)Value;
                MySqlCommand cmd = new MySqlCommand("select * from Login_Info where PlayerName = @name;", conn);
                cmd.Parameters.AddWithValue("@name", temp.PlayerName);
                cmd.Prepare();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        List<string> _params = new List<string>
                        {
                            reader["PlayerName"].ToString(),
                            reader["PlayerLogID"].ToString(),
                            reader["Password"].ToString()
                        };
                        list.Add((LoginObject)factory.GetDBContainer(_params));
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
                    LoginObject temp = (LoginObject)Value;
                    MySqlCommand cmd = new MySqlCommand("insert into Login_Info values(default, @Name, @Pass);", conn);
                    cmd.Parameters.AddWithValue("@Name", temp.PlayerName);
                    cmd.Parameters.AddWithValue("@Pass", temp.Pass);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    throw new Exception("Entry already exist");
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
                        LoginObject temp = (LoginObject)Value;
                        MySqlCommand cmd = new MySqlCommand("insert into Login_Info values(default, @Name, @Pass);", conn);
                        cmd.Parameters.AddWithValue("@Name", temp.PlayerName);
                        cmd.Parameters.AddWithValue("@Pass", temp.Pass);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        throw new Exception("One or more Entries already exist");
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
            LoginObject temp = (LoginObject)Value;
            MySqlCommand cmd = new MySqlCommand("select * from Login_Info where PlayerName = @name;", conn);
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
                            reader["PlayerLogID"].ToString(),
                            reader["Password"].ToString()
                        };
                    containers.Add((LoginObject)factory.GetDBContainer(_params));
                }
            }

            if(containers.Count > 0)
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
                LoginObject temp = (LoginObject)Value;
                MySqlCommand cmd = new MySqlCommand("Update Login_Info Set PlayerName = @name, Password = @pass where PlayerLogID = @ID;", conn);
                cmd.Parameters.AddWithValue("@ID", temp.PlayerID);
                cmd.Parameters.AddWithValue("@name", temp.PlayerName);
                cmd.Parameters.AddWithValue("@pass", temp.Pass);
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
                    LoginObject temp = (LoginObject)Value;
                    MySqlCommand cmd = new MySqlCommand("Update Login_Info Set PlayerName = @name, Password = @pass where PlayerLogID = @ID;", conn);
                    cmd.Parameters.AddWithValue("@ID", temp.PlayerID);
                    cmd.Parameters.AddWithValue("@name", temp.PlayerName);
                    cmd.Parameters.AddWithValue("@pass", temp.Pass);
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
                LoginObject temp = (LoginObject)Value;
                MySqlCommand cmd = new MySqlCommand("Delete from Login_Info where PlayerLogID = @ID;", conn);
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
                    LoginObject temp = (LoginObject)Value;
                    MySqlCommand cmd = new MySqlCommand("Delete from Login_Info where PlayerLogID = @ID;", conn);
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
