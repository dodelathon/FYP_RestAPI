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

        public ImageController(IHostingEnvironment env)
        {
            hostingEnvironment = env;
        }

        // POST: api/Imag
        [HttpPost("api/Image")]
        public async Task Post([FromForm]Image photo)
        {
            var actual_Picture = photo.photo;
            //if (actual_Picture != null)
            //{
                if (actual_Picture.Length > 0)
                {
                    var filePath = "wwwroot/" + actual_Picture.FileName;
                    System.IO.File.SetAttributes(filePath, FileAttributes.Normal);
                    Console.WriteLine(filePath);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                       await actual_Picture.CopyToAsync(fileStream);
                    }
                }
            //}
            //return Ok(new { status = true, message = "Photo Posted Successfully" });
        }
    }
}
