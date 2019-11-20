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
    [Route("api/CS4227/Player")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private DatabaseContext _context;
        private IAbFactory factory;
        public PlayerController(DatabaseContext context)
        {
            _context = context;
            factory = DBObjectAbstractFactory.GetFactory("Player");
        }


        [HttpPost("GetPlayerEntry")]
        public ActionResult<IEnumerable<IDBContainer>> GetStatsEntry([FromForm] string userName, [FromForm]string id, [FromForm] string highScore, [FromForm] string time)
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
                return _context.GetEntry("Player", temp);

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                return null;
            }
        }

        [HttpPost("AddPlayerEntry")]
        public string AddStatsEntry([FromForm] string userName, [FromForm] string id, [FromForm] string highScore, [FromForm] string time)
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
                _context.AddEntry("Player", temp);
                return "Success";
            }
            catch (Exception e)
            {
                return "failed\n" + e.Message + "\n" + e.StackTrace;
            }
        }

        [HttpPost("UpdatePlayerEntry")]
        public string UpdateStatsEntry([FromForm] string userName, [FromForm] string id, [FromForm] string highScore, [FromForm] string time)
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
                _context.UpdateEntry("Player", temp);
                return "success";
            }
            catch (Exception e)
            {
                return "failed\n" + e.Message + "\n" + e.StackTrace;
            }

        }

       

        [HttpPost("RemovePlayerEntry")]
        public string RemoveStatsEntry([FromForm] string userName, [FromForm]string id, [FromForm] string highScore, [FromForm] string time)
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
                _context.DeleteEntry("Player", temp);
                return "success";
            }
            catch (Exception e)
            {
                return "Failed\n" + e.Message + "\n" + e.StackTrace;
            }
        }
    }
}