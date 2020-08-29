using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FYP_3DPrinterMonitor.Models.Contexts;
using FYP_3DPrinterMonitor.Models.Containers;
using System.IO;
using Microsoft.AspNetCore.Hosting;


namespace FYP_3DPrinterMonitor.Controllers
{

    /* This class manages the submission and retrival of device statistics.
     */
    [Route("api/DeviceData/")]
    [ApiController]
    public class DeviceDataController : ControllerBase
    {
        private string filePath;
        private readonly ConnectedDevicesContext _context;

        //Constructor, requires reference to the database context, and hosting environment, Sets File paths
        public DeviceDataController(ConnectedDevicesContext context)
        {
            _context = context;
            filePath = "wwwroot/Stats/";
        }

        // Returns a list of all the devices that have been registered.
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

        //Requires a UUID and a File containing the Statistic information. Saves the file for later retrieval 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [HttpPost("UpdateDeviceStats")]
        public async Task<IActionResult> UpdateStats([FromHeader] string _Device, IFormFile StatsFile)
        {
            try
            {
                //Checks the file length and if the device exist before saving the file to the server.
                var actual_Stats = StatsFile;
                if (actual_Stats.Length > 0 && _context.Exists(_Device, ConnectedDevicesContext.DatabaseGetMode.UUID) == true)
                {
                    if (!Directory.Exists(filePath + _Device + "/"))
                    {
                        Directory.CreateDirectory(filePath + _Device + "/");
                    }

                    //Creates and stores the file.
                    using (var fileStream = new FileStream(filePath + _Device + "/" + actual_Stats.FileName, FileMode.Create))
                    {
                        System.IO.File.SetAttributes(filePath + _Device + "/" + actual_Stats.FileName, FileAttributes.Normal);
                        await actual_Stats.CopyToAsync(fileStream);
                    }
                    return Ok(new { status = true, message = "File Posted Successfully" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, _Device);
                }
            }
            catch(NullReferenceException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "No File Detected! ");
            }
        }
        //Method returns the information inside the Stats file. 
        [HttpGet("GetStats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetStats([FromHeader]string Device)
        {
            if (!(Directory.Exists(filePath + Device + "/")))
            {
                return NotFound("Device is not sending Statistics!");
            }
            else
            {
                Console.WriteLine(Device);

                //Iterates through the file to create a string as returning a JSON file created issues.
                var stats = System.IO.File.ReadLines(filePath + "/" + Device + "/" + "Stats.json");
                string retVal = "";
                foreach(string x in stats)
                {
                    retVal += x + "\n";
                }
                return Ok(retVal);
            }
        }
    }
}
