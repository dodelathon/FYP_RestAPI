﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;

using Microsoft.AspNetCore.Http;
namespace FYP_TestAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Image")]
    public class ImageController : Controller
    {
        private readonly IHostingEnvironment _environment;

        public ImageController(IHostingEnvironment environment)

        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        // POST: api/Imag
        [HttpPost]
        public async Task Post(IFormFile file)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))

                {

                    await file.CopyToAsync(fileStream);

                }
            }
        }
    }
}