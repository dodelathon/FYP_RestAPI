using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Autofac;
using CS4227_Database_API.Factories.Concrete_Factories;
using CS4227_Database_API.Factories.Abstract_Factories;
using CS4227_Database_API.Interceptors;
using Autofac.Extras.DynamicProxy;

namespace FYP_TestAPI
{
    public class Program
    {
        public static IAbFactory LeaderFac;
        public static IAbFactory LoginFac;
        public static IAbFactory PlayerFac;
        public static void Main(string[] args)
        {
            

            var Leaderbuilder = new ContainerBuilder();
            Leaderbuilder.RegisterType<LeaderboardObjectFactory>().As<IAbFactory>().EnableInterfaceInterceptors().InterceptedBy(typeof(FactoryCallLoggingInterceptor));
            Leaderbuilder.Register(LBI => new FactoryCallLoggingInterceptor(Console.Out));
            var Leadercontainer = Leaderbuilder.Build();
            LeaderFac = Leadercontainer.Resolve<IAbFactory>();

            var LoginBuilder = new ContainerBuilder();
            LoginBuilder.RegisterType<LoginObjectFactory>().As<IAbFactory>().EnableInterfaceInterceptors().InterceptedBy(typeof(FactoryCallLoggingInterceptor));
            LoginBuilder.Register(LGI => new FactoryCallLoggingInterceptor(Console.Out));
            var LoginContainer = LoginBuilder.Build();
            LoginFac = LoginContainer.Resolve<IAbFactory>();

            var PlayerBuilder = new ContainerBuilder();
            PlayerBuilder.RegisterType<PlayerObjectFactory>().As<IAbFactory>().EnableInterfaceInterceptors().InterceptedBy(typeof(FactoryCallLoggingInterceptor));
            PlayerBuilder.Register(PLI => new FactoryCallLoggingInterceptor(Console.Out));
            var PlayerContainer = PlayerBuilder.Build();
            PlayerFac = PlayerContainer.Resolve<IAbFactory>();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
