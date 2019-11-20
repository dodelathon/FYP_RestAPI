using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS4227_Database_API.Factories.Abstract_Factories;

namespace CS4227_Database_API.Models.Containers
{
    public class LeaderboardObject :IDBContainer
    {
        public static LeaderboardObject Creator(object caller, string _name, string _ID, string _highscore, string _bestTime)
        {
            IAbFactory temp = (IAbFactory)caller;
            if(temp != null)
            {
                return new LeaderboardObject(_name, _ID, _highscore, _bestTime);
            }
            else
            {
                return null;
            }
        }
        private LeaderboardObject (string _name, string _ID, string _highscore, string _Time)
        {
            PlayerName = _name;
            PlayerID = int.Parse(_ID);
            Highscore = int.Parse(_highscore);
            ScoreTime = int.Parse(_Time);
        }
        public string PlayerName { get; set; }
        public int PlayerID { get; set; }
        public int Highscore { get; set; }
        public int ScoreTime { get; set; }
    }
}
