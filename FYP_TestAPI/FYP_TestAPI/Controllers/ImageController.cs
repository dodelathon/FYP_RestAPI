using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using FYP_TestAPI.Models;
using Microsoft.AspNetCore.Http;

namespace FYP_TestAPI.Controllers
{
    [Route("api/Image/")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        private IHostingEnvironment hostingEnvironment;
        private string filePath;

        public ImageController(IHostingEnvironment env)
        {
            hostingEnvironment = env;
            filePath = "Images/";
            Console.WriteLine(env.EnvironmentName);
        }

        // POST: api/Imag
        [HttpPost("Upload")]
        public async Task RecieveImage([FromForm]Image recieved)
        {
            var actual_Picture = recieved.photo;
            //if (actual_Picture != null)
            //{
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
            }
            //}
            //return Ok(new { status = true, message = "Photo Posted Successfully" });
        }

        [HttpGet("GetImage")]
        public IActionResult GetImage(string Device)
        {
            Console.WriteLine(Device);
            Byte[] image = System.IO.File.ReadAllBytes(filePath + "image.jpg"); 
            return File(image, "image/jpeg");
        }   
    }
}
