using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using CS4227_Database_API.Models.Containers;
using CS4227_Database_API.Models.DBContexts.Interaction_Classes;
using CS4227_Database_API.Factories;


namespace CS4227_Database_API.Models.DBContexts
{
    public class DatabaseContext
    {
        private string ConnectionString { get; }
        private Factories.Abstract_Factories.IAbFactory Factory;
        private IInteraction interaction;
        public static MySqlConnection conn;

        public DatabaseContext(string cString)
        {
            ConnectionString = cString;
            conn = GetConnection();
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<IDBContainer> GetAll(string Table)
        {
            List<IDBContainer> list = new List<IDBContainer>();
            try
            {
                Setter(Table, list);
                interaction.GetAll();
                return list;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<IDBContainer> GetMultiple(string Table, List<IDBContainer> values)
        {
            List<IDBContainer> list = new List<IDBContainer>();
            try
            {
                Setter(Table, list);
                interaction.GetMultipleEntries (values);
                return list;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<IDBContainer> GetEntry(string Table, IDBContainer value)
        {
            List<IDBContainer> list = new List<IDBContainer>();
            try
            {
                Setter(Table,list);
                interaction.GetPlayerEntry(value);
                return list;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void AddEntry(string Table, IDBContainer Value)
        {
            try
            {
                Setter(Table);
                interaction.AddEntry(Value);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void AddEntry(string Table, List<IDBContainer> Values)
        {
            try
            {
                Setter(Table);
                interaction.AddEntry(Values);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void UpdateEntry(string Table, IDBContainer Value)
        {
            try
            {
                Setter(Table);
                interaction.UpdateEntry(Value);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void UpdateEntry(string Table, List<IDBContainer> Values)
        {
            try
            {
                Setter(Table);
                interaction.UpdateEntry(Values);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void DeleteEntry(string Table, IDBContainer Value)
        {
            try
            {
                Setter(Table);
                interaction.DeleteEntry(Value);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void DeleteEntry(string Table, List<IDBContainer> Values)
        {
            try
            {
                Setter(Table);
                interaction.DeleteEntry(Values);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Initialization_Area
        private void Setter(string Table, List<IDBContainer> list)
        {
            conn.Open();
            switch (Table)
            {
                case "Leaderboard": Factory = Factories.Abstract_Factories.DBObjectAbstractFactory.GetFactory("Leaderboard"); interaction = new LeaderboardInteraction(conn, Factory, list); break;
                case "Login": Factory = Factories.Abstract_Factories.DBObjectAbstractFactory.GetFactory("Login"); interaction = new LoginInteraction(conn, Factory, list); break;
                case "Player": Factory = Factories.Abstract_Factories.DBObjectAbstractFactory.GetFactory("Player"); interaction = new PlayerInteraction(conn, Factory, list); break;
            }
        }

        private void Setter(string Table)
        {
            conn.Open();
            switch (Table)
            {
                case "Leaderboard": Factory = Factories.Abstract_Factories.DBObjectAbstractFactory.GetFactory("Leaderboard"); interaction = new LeaderboardInteraction(conn, Factory); break;
                case "Login": Factory = Factories.Abstract_Factories.DBObjectAbstractFactory.GetFactory("Login"); interaction = new LoginInteraction(conn, Factory); break;
                case "Player": Factory = Factories.Abstract_Factories.DBObjectAbstractFactory.GetFactory("Player"); interaction = new PlayerInteraction(conn, Factory); break;
            }
        }
        #endregion
    }
}
