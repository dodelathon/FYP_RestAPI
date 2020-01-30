using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_TestAPI.Models.Containers
{
    public class DevStatsContainer
    {
        public double _temp { get; set; } 
        public double _progress { get; set; }
        public string _CurrentPrintName { get; set; }
        public string _Owner { get; set; }

    }
}
