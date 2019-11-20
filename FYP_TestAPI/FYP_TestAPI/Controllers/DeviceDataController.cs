using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYP_TestAPI.Models;

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

        [HttpGet]
        public ActionResult<IEnumerable<FeederDevice>> GetAllDevices()
        {
            return _context.GetAllDevices();
        }

    }
}
