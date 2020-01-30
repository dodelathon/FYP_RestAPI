using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using FYP_TestAPI.Models.Containers;
using FYP_TestAPI.Models.Contexts;
using Microsoft.AspNetCore.Http;
using System.Runtime.InteropServices;

namespace FYP_TestAPI.Controllers
{
    [Route("api/Image/")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        private IHostingEnvironment hostingEnvironment;
        private string filePath;
        ConnectedDevicesContext _context;

        public ImageController(IHostingEnvironment env, ConnectedDevicesContext context)
        {
            _context = context;
            hostingEnvironment = env;
            filePath = "wwwroot/images/";
            //Console.WriteLine(env.EnvironmentName);
        }

        // POST: api/Imag
        [HttpPost("Upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RecieveImage([FromForm]Image recieved)
        {
            var actual_Picture = recieved.photo;
            if (_context.GetDevice(recieved._DevID) != null)
            {
                if (actual_Picture.Length > 0)
                {
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    filePath += actual_Picture.FileName;
                    System.IO.File.SetAttributes(filePath, FileAttributes.Normal);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await actual_Picture.CopyToAsync(fileStream);
                    }
                    return Ok(new { status = true, message = "Photo Posted Successfully" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable);
                }
            }
            else
            {
                return NotFound("Device not registered");
            }
                      
        }

        [HttpGet("GetImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetImage(string Device)
        {
            if (!(Directory.Exists(filePath + "/" + Device)))
            { 
                return NotFound("Device is not Registered or is not sending Images!");
            }
            else
            {
                Console.WriteLine(Device);
                byte[] image = System.IO.File.ReadAllBytes(filePath + "image.jpg");
                return Ok(File(image, "image/jpeg"));
            }
        }   
    }
}
