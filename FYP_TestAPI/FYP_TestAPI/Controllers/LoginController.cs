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
    [Route("api/Cs4227/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private DatabaseContext _context;
        private IAbFactory factory;
        public LoginController(DatabaseContext context)
        {
            _context = context;
            factory = DBObjectAbstractFactory.GetFactory("Login");
        }
        #endregion

        #region Potential_Getters
        /*
        [HttpGet("GetAllLeaderboardEntries")]
        public ActionResult<IEnumerable<IDBContainer>> GetAllLeaderboardEntries()
        {
            try
            {
                return _context.GetAll("Login_Info");
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("GetMultipleLeaderboardEntries")]
        public ActionResult<IEnumerable<IDBContainer>> GetMultipleLoginEntries([FromForm] string MultiEntry)
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

                return _context.GetMultiple("Login_Info", Entries);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                return null;
            }
        }
        */
        #endregion

        #region Getter
        [HttpPost("Login")]
        public ActionResult<IEnumerable<IDBContainer>> GetLoginEntry([FromForm] string userName, [FromForm]string Pass)
        {
            try
            {
                List<string> _params = new List<string>
                {
                    userName,
                    "0",
                    Pass
                };
                IDBContainer temp = factory.GetDBContainer(_params);
                List<IDBContainer> res = _context.GetEntry("Login", temp);
                if (res.Count != 0 )
                {
                    Console.WriteLine("res not 0");
                    if (((LoginObject)res[0]).Pass.Equals(((LoginObject)temp).Pass))
                    {
                        foreach (LoginObject log in res)
                        {
                            log.Pass = "";
                        }
                        return res;
                    }
                }
                return null;      
            }
            catch
            {
                Console.WriteLine("We broke");
                return null;
            }
        }
        #endregion

        #region Adder
        [HttpPost("AddLoginEntry")]
        public string AddLoginEntry([FromForm] string userName, [FromForm] string Pass)
        {
            try
            {
                List<string> _params = new List<string>
                    {
                        userName,
                        "0",
                        Pass

                    };
                IDBContainer temp = factory.GetDBContainer(_params);
                _context.AddEntry("Login", temp);
                return "Success";
            }
            catch (Exception e)
            {
                return "failed\n" + e.Message + "\n" + e.StackTrace;
            }
        }

        #endregion

        #region Updater
        [HttpPost("UpdateLoginEntry")]
        public string UpdateLoginEntry([FromForm] string userName, [FromForm]string id, [FromForm] string Pass)
        {
            try
            {
                List<string> _params = new List<string>
                {
                    userName,
                    id,
                    Pass
                };
                IDBContainer temp = factory.GetDBContainer(_params);
                _context.UpdateEntry("Login", temp);
                return "success";
            }
            catch(Exception e)
            {
                return "failed\n" + e.Message + "\n" + e.StackTrace;
            }

        }
        #endregion

        #region Potential_Updater
        /*
        [HttpPost("UpdateMultipleLoginEntries")]
        public string UpdateMultipleLoginEntries([FromForm] string MultiEntry)
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

                _context.UpdateEntry("Login_Info", Entries);
                return "success";
            }
            catch
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

                    _context.AddEntry("Login_Info", Entries);
                    RemoveDuplicates("Login_Info");

                    return "success";
                }
                catch (Exception e)
                {
                    return "failed\n" + e.Message + "\n" + e.StackTrace;
                }
            }

        }*/
        #endregion

        #region Deleter
        [HttpPost("RemoveLoginEntry")]
        public string RemoveLoginEntry([FromForm] string userName, [FromForm]string id, [FromForm] string Pass)
        {
            try
            {
                List<string> _params = new List<string>
                {
                    userName,
                    id,
                    Pass
                };
                IDBContainer temp = factory.GetDBContainer(_params);
                _context.DeleteEntry("Login", temp);
                return "success";
            }
            catch (Exception e)
            {
                return "Failed\n" + e.Message + "\n" + e.StackTrace;
            }
        }
        #endregion

        #region Potential_Remover
        /*
        [HttpPost("RemoveMultipleLoginEntries")]
        public string RemoveMultipleLoginEntries([FromForm] string MultiEntry)
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

                _context.DeleteEntry("Login_Info", Entries);
                return "success";
            }
            catch
            {
                return "Failed";
            }

        }*/
        #endregion

    }
}