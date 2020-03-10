using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FYP_TestAPI.Models.Contexts;
using FYP_TestAPI.Models.Containers;
using System.IO;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP_TestAPI.Controllers
{
    [Route("api/DeviceData/")]
    [ApiController]
    public class DeviceDataController : ControllerBase
    {
        private IHostingEnvironment hostingEnvironment;
        private string filePath;
        private readonly ConnectedDevicesContext _context;
        public DeviceDataController(ConnectedDevicesContext context, IHostingEnvironment env)
        {
            _context = context;
            hostingEnvironment = env;
            filePath = "wwwroot/Stats/";
        }

        [HttpGet("GetAllDevices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<FeederDevice>> GetAllDevices()
        {
            try
            {
                return Ok(_context.GetAllDevices());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [HttpPost("UpdateDeviceStats")]
        public async Task<IActionResult> UpdateStats([FromForm]DeviceStatsContainer stats)
        {

            var actual_Stats = stats.StatsFile;
            if (actual_Stats.Length > 0 && _context.Exists(stats._Device, ConnectedDevicesContext.DatabaseGetMode.UUID) == true)
            {
                if (!Directory.Exists(filePath + stats._Device + "/"))
                {
                    Directory.CreateDirectory(filePath + stats._Device + "/");
                }
                using (var fileStream = new FileStream(filePath + stats._Device + "/" + actual_Stats.FileName, FileMode.Create))
                {
                    System.IO.File.SetAttributes(filePath + stats._Device + "/" + actual_Stats.FileName, FileAttributes.Normal);
                    await actual_Stats.CopyToAsync(fileStream);
                }
                return Ok(new { status = true, message = "File Posted Successfully" });
            }
            else
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
        }

        [HttpGet("GetStats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetImage(string Device)
        {
            if (!(Directory.Exists(filePath + "/" + Device)))
            {
                return NotFound("Device is not sending Statistics!");
            }
            else
            {
                Console.WriteLine(Device);
                string[] stats = System.IO.File.ReadLines(filePath + "/" + Device + "/" + "Stats.json").ToArray();
                return Ok(stats);
            }
        }
    }
}
