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

    public class ImageController : ControllerBase
    {

        private IHostingEnvironment hostingEnvironment;
        private string filePath = "wwwroot/images/";

        public ImageController(IHostingEnvironment env)
        {
            hostingEnvironment = env;
            Console.WriteLine(env.EnvironmentName);
        }

        // POST: api/Imag
        [HttpPost("api/Image/Upload")]
        public async Task Post([FromForm]Image photo)
        {
            var actual_Picture = photo.photo;
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

        [HttpGet("api/image/GetImage")]
        public IActionResult GetImage(string DeviceString)
        {
            Byte[] image = System.IO.File.ReadAllBytes(filePath + "image.jpg");
            return File(image, "image/jpeg");
        }   
    }
}
