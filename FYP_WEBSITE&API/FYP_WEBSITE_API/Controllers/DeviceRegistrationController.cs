using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FYP_3DPrinterMonitor.Models.Contexts;
using FYP_3DPrinterMonitor.Models.Containers;
using System.IO;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP_3DPrinterMonitor.Controllers
{
    /* Class contains the Device registration functionality. 
     */
    [Route("api/DeviceRegistration/")]
    [ApiController]
    public class DeviceRegistrationController : ControllerBase
    {
        private IHostingEnvironment hostingEnvironment;
        private string StatsfilePath;
        private string ImagefilePath;
        private readonly ConnectedDevicesContext _context;

        //Constructor, requires reference to the database context, and hosting environment, Sets File paths
        public DeviceRegistrationController(ConnectedDevicesContext context, IHostingEnvironment env)
        {
            _context = context;
            hostingEnvironment = env;
            StatsfilePath = "wwwroot/Stats/";
            ImagefilePath = "wwwroot/images/";
        }
       
        //Method returns if a device is registered, Requires the devices name.
        [HttpGet("IsRegistered")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult IsRegistered(string name)
        {
            try
            {
                FeederDevice temp = _context.GetDevice(name, ConnectedDevicesContext.DatabaseGetMode.Name);
                if (temp != null)
                {
                    return Ok(temp.UUID);
                }
                else
                {
                    return NotFound(false);
                }
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        // Method that allows a device to be registered, requires a name, and returns a UUID for the device.
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Register([FromForm]string DevName)
        {
            try
            {
                Guid UUID = Guid.NewGuid();
                if (_context.AddDevice(DevName, UUID.ToString()) == true)
                {
                    return Ok(UUID.ToString());
                }
                else
                {
                    return StatusCode(StatusCodes.Status409Conflict, (_context.GetDevice(DevName, ConnectedDevicesContext.DatabaseGetMode.Name)).UUID);
                }
                
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message + "\n" + e.StackTrace);
            }
        }

        // Method to remove a device, Requires the UUID, will remove all server side paths/files related to the device, and the database entry.
        [HttpPost("RemoveDevice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult RemoveDevice([FromForm]string UUID)
        {
            try
            {
                //Checks and removes the statistics files and paths if they exist.
                if (Directory.Exists(StatsfilePath + UUID + "/"))
                {
                    var files = Directory.GetFiles(StatsfilePath + UUID + "/");
                    foreach(string x in files)
                    {
                        System.IO.File.Delete(x);
                    }
                    Directory.Delete(StatsfilePath + UUID + "/");
                }

                //Checks and removes the image files and paths if they exist.
                if (Directory.Exists(ImagefilePath + UUID + "/"))
                {
                    var files = Directory.GetFiles(ImagefilePath + UUID + "/");
                    foreach (string x in files)
                    {
                        System.IO.File.Delete(x);
                    }
                    Directory.Delete(ImagefilePath + UUID + "/");
                }

                // Removes the device form the database, else returns that there was a databse issue.
                if (_context.RemoveDevice(UUID) == true)
                {
                    return Ok("Device Successully Removed!");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal Error Has Occured While Deleting Device!");
                }

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message + "\n" + e.StackTrace);
            }
        }
    }
}
