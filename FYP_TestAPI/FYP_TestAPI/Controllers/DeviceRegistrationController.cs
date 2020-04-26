using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FYP_TestAPI.Models.Contexts;
using FYP_TestAPI.Models.Containers;
using System.IO;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP_TestAPI.Controllers
{
    [Route("api/DeviceRegistration/")]
    [ApiController]
    public class DeviceRegistrationController : ControllerBase
    {
        private IHostingEnvironment hostingEnvironment;
        private string StatsfilePath;
        private string ImagefilePath;
        private readonly ConnectedDevicesContext _context;
        public DeviceRegistrationController(ConnectedDevicesContext context, IHostingEnvironment env)
        {
            _context = context;
            hostingEnvironment = env;
            StatsfilePath = "wwwroot/Stats/";
            ImagefilePath = "wwwroot/images/";
        }
       
        // GET: api/<controller>
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


        // POST api/<controller>
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

        [HttpPost("RemoveDevice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult RemoveDevice([FromForm]string UUID)
        {
            try
            {
                if (Directory.Exists(StatsfilePath + UUID + "/"))
                {
                    Directory.Delete(StatsfilePath + UUID + "/");
                }

                if (Directory.Exists(ImagefilePath + UUID + "/"))
                {
                    Directory.Delete(ImagefilePath + UUID + "/");
                }

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
