using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS4227_Database_API.Models.Containers;
using CS4227_Database_API.Factories.Concrete_Factories;
using FYP_TestAPI;

namespace CS4227_Database_API.Factories.Abstract_Factories
{
    public abstract class DBObjectAbstractFactory: IAbFactory
    {
        //Must be overridden by derived classes
        public abstract IDBContainer GetDBContainer(List<string> _params);
        public static IAbFactory GetFactory(string DBAction)
        {
            IAbFactory returnable;
            switch (DBAction)
            {
                case "Login":
                    returnable = Program.LoginFac;
                    break;
                case "Leaderboard":
                    returnable = Program.LeaderFac;
                    break;
                case "Player":
                    returnable = Program.PlayerFac;
                    break;
                default:
                    returnable = null;
                    break;
            }

            return returnable;
        }
    }
}
