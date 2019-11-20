using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS4227_Database_API.Factories.Abstract_Factories;

namespace CS4227_Database_API.Models.Containers
{
    public class PlayerObject : IDBContainer
    {
        public static PlayerObject Creator(object caller, string _name, string _ID, string _recentScore, string _recenttime)
        {
            IAbFactory temp = (IAbFactory)caller;
            if (temp != null)
            {
                return new PlayerObject(_name, _ID,  _recenttime, _recentScore);
            }
            else
            {
                return null;
            }
        }
        private PlayerObject(string _name, string _ID,  string _recenttime, string _recentScore)
        {
            PlayerName = _name;
            PlayerID = int.Parse(_ID);
            RecentTime = int.Parse(_recenttime);
            RecentScore = int.Parse(_recentScore);

        }
        public string PlayerName { get; set; }
        public int PlayerID { get; set; }
        public int RecentScore { get; set; }
        public int RecentTime { get; set; }
    }
}
