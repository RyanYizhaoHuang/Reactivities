﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host =  CreateWebHostBuilder(args).Build();

            // using means: auto collect garbage after running code
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try {
                    // update database
                    var context = services.GetRequiredService<DataContext>();
                    context.Database.Migrate();
                }
                catch(Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured during migration");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
