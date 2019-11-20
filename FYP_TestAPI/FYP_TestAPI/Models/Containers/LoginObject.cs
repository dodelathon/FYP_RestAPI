using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS4227_Database_API.Factories.Abstract_Factories;

namespace CS4227_Database_API.Models.Containers
{
    public class LoginObject : IDBContainer
    {
        public static LoginObject Creator(object caller, string _name, string _ID, string _Pass)
        {
            IAbFactory temp = (IAbFactory)caller;
            if (temp != null)
            {
                return new LoginObject(_name, _ID, _Pass);
            }
            else
            {
                return null;
            }
        }
        private LoginObject(string _name, string _Id, string _pass)
        {
            PlayerName = _name;
            PlayerID = int.Parse(_Id);
            Pass = _pass;
        }
        public string PlayerName { get; set; }
        public int PlayerID { get; set; }
        public string Pass { get; set; }
    }
}
