using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;

namespace TesteBari.Api
{
    public class Program
    {
        static ILogger Logger;

        public static void Main(string[] args)
        {
            var levelSwitch = new LoggingLevelSwitch(LogEventLevel.Information);

            // Configure Serilog for logging
            Logger = new LoggerConfiguration()
           .MinimumLevel.ControlledBy(levelSwitch)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
           .CreateLogger();

            try
            {
                Log.Information("Iniciando Api");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Api falhou na inicialização");
            }
            finally
            {
                Log.CloseAndFlush();
            }
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog(Logger);
    }
}
