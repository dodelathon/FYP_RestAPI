using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FYP_TestAPI.Models.Contexts;
using FYP_TestAPI.Models.Containers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP_TestAPI.Controllers
{
    [Route("api/DeviceRegistration/")]
    [ApiController]
    public class DeviceRegistrationController : ControllerBase
    {
        private readonly ConnectedDevicesContext _context;
        public DeviceRegistrationController(ConnectedDevicesContext context)
        {
            _context = context;
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
                    return StatusCode(StatusCodes.Status409Conflict, (_context.GetDevice(UUID.ToString(), ConnectedDevicesContext.DatabaseGetMode.UUID)).UUID);
                }
                
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message + "\n" + e.StackTrace);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
