using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_TestAPI.Models.Containers
{
    public class DeviceStatsContainer
    {
        public IFormFile StatsFile { get; set; }
        public string _Device { get; set; }

    }
}
