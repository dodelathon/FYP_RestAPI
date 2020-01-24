using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FYP_TestAPI.Models.Contexts;
using FYP_TestAPI.Models.Containers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP_TestAPI.Controllers
{
    [Route("api/DeviceData/")]
    [ApiController]
    public class DeviceDataController : ControllerBase
    {
        private readonly ConnectedDevicesContext _context;
        public DeviceDataController(ConnectedDevicesContext context)
        {
            _context = context;
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

        [HttpPut("UpdateDeviceStats")]
        public ActionResult UpdateStats()
        {
            try
            {
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
