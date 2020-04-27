using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace FYP_3DPrinterMonitor
{

    /* Main program for the API
     */
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
