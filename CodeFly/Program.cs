using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CodeFly
{
    public class Program
    {
        static string Ip { get; set; }
        static int Port { get; set; }

        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddCommandLine(args).Build();
            Ip = config.GetValue<string>("ip") ?? "0.0.0.0";
            Port = config.GetValue<int?>("port") ?? 5000;
            // var httpsPort = config.GetValue<int?>("https") ?? 5005;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel(options =>
                    {

                        // HTTP 5000
                        options.ListenLocalhost(5000);

                        // HTTPS 5001
                        options.ListenLocalhost(5001, builder =>
                        {
                            builder.UseHttps();
                        });
                    });
                    // webBuilder.UseKestrel(options =>
                    // {
                    //     options.Limits.MaxRequestBodySize = 10000000;
                    //     options.Listen(IPAddress.Any, Port, op =>
                    //     {
                    //         options.Limits.MaxConcurrentConnections = null;
                    //         options.Limits.MaxConcurrentUpgradedConnections = null;
                    //     });
                    // });
                });
    }
}