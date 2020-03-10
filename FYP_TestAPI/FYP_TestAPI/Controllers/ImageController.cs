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

namespace FYP_TestAPI.Controllers
{
    [Route("api/Image/")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        private IHostingEnvironment hostingEnvironment;
        private string filePath;
        private ConnectedDevicesContext _context;

        public ImageController(ConnectedDevicesContext conn, IHostingEnvironment env)
        {
            hostingEnvironment = env;
            filePath = "wwwroot/images/";
            _context = conn;
            //Console.WriteLine(env.EnvironmentName);
        }

        // POST: api/Imag
        [HttpPost("Upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<IActionResult> RecieveImage([FromForm]Image recieved)
        {
            var actual_Picture = recieved.photo;
            try
            {
                if (actual_Picture.Length > 0 && _context.Exists(recieved._Device, ConnectedDevicesContext.DatabaseGetMode.UUID) == true)
                {
                    if (!Directory.Exists(filePath + recieved._Device + "/"))
                    {
                        Directory.CreateDirectory(filePath + recieved._Device + "/");
                    }
                    using (var fileStream = new FileStream(filePath + recieved._Device + "/" + actual_Picture.FileName, FileMode.Create))
                    {
                        System.IO.File.SetAttributes(filePath + recieved._Device + "/" + actual_Picture.FileName, FileAttributes.Normal);
                        await actual_Picture.CopyToAsync(fileStream);
                    }
                    return Ok(new { status = true, message = "Photo Posted Successfully" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable);
                }
            }
            catch (NullReferenceException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "No Image Detected");
            }

        }

        [HttpGet("GetImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetImage(string Device)
        {
            if (!(Directory.Exists(filePath + "/" + Device + "/")))
            { 
                return NotFound("Device is not Registered!");
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
