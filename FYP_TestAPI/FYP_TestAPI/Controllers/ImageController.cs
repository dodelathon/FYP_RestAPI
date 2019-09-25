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

        // POST: api/Imag
        [HttpPost("api/Image")]
        public ActionResult Post([FromForm]Image photo)
        {
            var actual_Picture = photo.photo;
            //if (actual_Picture != null)
            //{
            System.IO.Directory.CreateDirectory("C:\\Users\\donal\\source\\repos\\FYP_WebApi\\FYP_TestAPI\\FYP_TestAPI\\wwwroot\\images");
                if (actual_Picture.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/images", actual_Picture.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                       actual_Picture.CopyTo(fileStream);
                    }
                }
            //}
            return Ok(new { status = true, message = "Photo Posted Successfully" });
        }
    }
}