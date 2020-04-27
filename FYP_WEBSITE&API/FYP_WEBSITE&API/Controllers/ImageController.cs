using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using FYP_3DPrinterMonitor.Models.Contexts;
using Microsoft.AspNetCore.Http;

namespace FYP_3DPrinterMonitor.Controllers
{
    /* This class manages the submission and retrival of device Images.
    */
    [Route("api/Image/")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        private IHostingEnvironment hostingEnvironment;
        private string filePath;
        private ConnectedDevicesContext _context;

        //Constructor, requires reference to the database context, and hosting environment, Sets File paths
        public ImageController(ConnectedDevicesContext conn, IHostingEnvironment env)
        {
            hostingEnvironment = env;
            filePath = "wwwroot/images/";
            _context = conn;
            //Console.WriteLine(env.EnvironmentName);
        }

        //Requires a UUID and a File containing the Image. Saves the Image for later retrieval 
        [HttpPost("Upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<IActionResult> RecieveImage([FromHeader]string _Device, IFormFile photo)
        {
            var actual_Picture = photo;
            try
            {
                //Checks the file length and if the device exist before saving the file to the server.
                if (actual_Picture.Length > 0 && _context.Exists(_Device, ConnectedDevicesContext.DatabaseGetMode.UUID) == true)
                {
                    if (!Directory.Exists(filePath + _Device + "/"))
                    {
                        Directory.CreateDirectory(filePath + _Device + "/");
                    }

                    //Creates and stores the file.
                    using (var fileStream = new FileStream(filePath + _Device + "/" + actual_Picture.FileName, FileMode.Create))
                    {
                        System.IO.File.SetAttributes(filePath + _Device + "/" + actual_Picture.FileName, FileAttributes.Normal);
                        await actual_Picture.CopyToAsync(fileStream);
                    }
                    return Ok(new { status = true, message = "Photo Posted Successfully" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, "Does not exist: " + _context.Exists(_Device, ConnectedDevicesContext.DatabaseGetMode.UUID) + " Name: " + _Device + " or length is unsuitable: " + actual_Picture.Length);
                }
            }
            catch (NullReferenceException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "No Image Detected");
            }

        }

        //Method requires a UUID, and returns the most recent image recieved for a device.
        [HttpGet("GetImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetImage(string Device)
        {
            if (!(Directory.Exists(filePath + "/" + Device + "/")))
            { 
                return NotFound("Device is not Sending Images");
            }
            else
            {
                //Writes the image data into a byte array to be returned and then parsed back into an image.
                Console.WriteLine(Device);
                byte[] image = System.IO.File.ReadAllBytes(filePath + "/" + Device + "/" + "image.jpg");
                return Ok(File(image, "image/jpeg"));
            }
        }   
    }
}
