﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FYP_TestAPI.Models.Containers
{
    public class Image
    {
        public IFormFile photo { get; set; }
        public string _DevID { get; set; }
    }
}
