using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CS4227_Database_API.Models.Containers;
using CS4227_Database_API.Models.DBContexts;
using CS4227_Database_API.Factories.Abstract_Factories;

namespace CS4227_Database_API.Controllers
{
    #region Class_Elements
    [Route("api/CS4227/Leaderboard")]
    [ApiController]
    public class LeaderBoardController : ControllerBase
    {
        private IAbFactory factory;
        private DatabaseContext _context;
        public LeaderBoardController(DatabaseContext context)
        {
            _context = context;
            factory = DBObjectAbstractFactory.GetFactory("Leaderboard");
        }
    #endregion


        [HttpGet("GetAllLeaderboardEntries")]
        public ActionResult<IEnumerable<IDBContainer>> GetAllLeaderboardEntries()
        {
            try
            {
                return _context.GetAll("Leaderboard");
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("GetMultipleLeaderboardEntries")]
        public ActionResult<IEnumerable<IDBContainer>> GetMultipleLeaderboardEntries([FromForm] string MultiEntry)
        {
            try
            {
                string[] splitter = MultiEntry.Split(",");
                List<IDBContainer> Entries = new List<IDBContainer>();
                for (int i = 0; i < splitter.Length; i += 4)
                {
                    List<string> _params = new List<string>
                    {
                        splitter[i],
                        splitter[i + 1],
                        splitter[i + 2],
                        splitter[i + 3]
                    };
                    Entries.Add(factory.GetDBContainer(_params));
                }

                return _context.GetMultiple("Leaderboard", Entries);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace );
                return null;
            }
        }

        [HttpPost("GetPlayerLeaderboardEntry")]
        public ActionResult<IEnumerable<IDBContainer>> GetPlayerLeaderboardEntry([FromForm] string userName, [FromForm]string id, [FromForm] string highScore, [FromForm] string time)
        {
            try
            {
                List<string> _params = new List<string>
                {
                    userName,
                    id,
                    highScore,
                    time
                };
                IDBContainer temp = factory.GetDBContainer(_params);
                return _context.GetEntry("Leaderboard", temp);
                
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("AddLeaderboardEntry")]
        public string AddLeaderboardEntry([FromForm] string userName, [FromForm] string id, [FromForm] string highScore, [FromForm] string time)
        {
            try
            {
                List<string> _params = new List<string>
                    {
                        userName,
                        id,
                        highScore,
                        time
                    };
                IDBContainer temp = factory.GetDBContainer(_params);
                _context.AddEntry("Leaderboard", temp);
                return "Success";
            }
            catch (Exception e)
            {
                return "failed\n" + e.Message + "\n" + e.StackTrace;
            }
        }

        [HttpPost("AddMultipleLeaderboardEntries")]
        public string AddMultipleLeaderboardEntry([FromForm] string MultiEntry)
        {
            try
            {
                string[] splitter = MultiEntry.Split(",");
                List<IDBContainer> Entries = new List<IDBContainer>();
                for (int i = 0; i < splitter.Length; i += 4)
                {
                    List<string> _params = new List<string>
                        {
                            splitter[i],
                            splitter[i + 1],
                            splitter[i + 2],
                            splitter[i + 3]
                        };
                    Entries.Add(factory.GetDBContainer(_params));
                }

                _context.AddEntry("Leaderboard", Entries);

                return "success";
            }
            catch (Exception e)
            {
                return "failed\n" + e.Message + "\n" + e.StackTrace;
            }
        }

        [HttpPost("UpdateLeaderboardEntry")]
        public string UpdateLeaderboardEntry([FromForm] string userName, [FromForm] string id, [FromForm] string highScore, [FromForm] string time)
        {
            try
            {
                List<string> _params = new List<string>
                {
                    userName,
                    id,
                    highScore,
                    time
                };
                IDBContainer temp = factory.GetDBContainer(_params);
                return "success";
            }
            catch (Exception e)
            {
                return "failed\n" + e.Message + "\n" + e.StackTrace;
            }

        }

        [HttpPost("UpdateMultipleLeaderboardEntries")]
        public string UpdateMultipleLeaderboardEntries([FromForm] string MultiEntry)
        {
            try
            {
                string[] splitter = MultiEntry.Split(",");
                List<IDBContainer> Entries = new List<IDBContainer>();
                for (int i = 0; i < splitter.Length; i += 4)
                {
                    List<string> _params = new List<string>
                    {
                        splitter[i],
                        splitter[i + 1],
                        splitter[i + 2],
                        splitter[i + 3]
                    };
                    Entries.Add(factory.GetDBContainer(_params));
                }

                _context.UpdateEntry("Leaderboard", Entries);
                return "success";
            }
            catch (Exception e)
            {
                return "failed\n" + e.Message + "\n" + e.StackTrace;
            }

        }

        [HttpPost("RemoveLeaderboardEntry")]
        public string RemoveLeaderboardEntry([FromForm] string userName, [FromForm]string id, [FromForm] string highScore, [FromForm] string time)
        {
            try
            {
                List<string> _params = new List<string>
                {
                    userName,
                    id,
                    highScore,
                    time
                };
                IDBContainer temp = factory.GetDBContainer(_params);
                _context.DeleteEntry("Leaderboard", temp);
                return "success";
            }
            catch(Exception e)
            {
                return "Failed\n" + e.Message + "\n" + e.StackTrace;
            }
        }

        [HttpPost("RemoveMultipleLeaderboardEntries")]
        public string RemoveMultipleLeaderboardEntries([FromForm] string MultiEntry)
        {
            try
            {
                string[] splitter = MultiEntry.Split(",");
                List<IDBContainer> Entries = new List<IDBContainer>();
                for (int i = 0; i < splitter.Length; i += 4)
                {
                    List<string> _params = new List<string>
                    {
                        splitter[i],
                        splitter[i + 1],
                        splitter[i + 2],
                        splitter[i + 3]
                    };
                    Entries.Add(factory.GetDBContainer(_params));
                }

                _context.DeleteEntry("Leaderboard", Entries);
                return "success";
            }
            catch (Exception e)
            {
                return "failed\n" + e.Message + "\n" + e.StackTrace;
            }

        }
    }
}