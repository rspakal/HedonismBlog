using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Filters;
using NLog.Targets;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HedonismBlog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var loggingConfig = new LoggingConfiguration();
            var logFile = new FileTarget("logFile") { FileName = "logs/${date:format=yyyy-MM-dd}_logs.txt" };
            var rule = new LoggingRule("*", NLog.LogLevel.Info, logFile);
            rule.Filters.Add(new ConditionBasedFilter
            {
                Condition = "contains('${message}', 'requested URL') or contains('${message}', 'User action') or contains('${message}', 'An error occurred')",

                Action = FilterResult.Log
            });
            loggingConfig.LoggingRules.Add(rule);

            
            LogManager.Configuration = loggingConfig;

            var logger = NLog.LogManager.GetCurrentClassLogger();
            try
            {
                logger.Debug("Init of the app");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "App stoped becouse of ex");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            })
            .UseNLog();
    }
}
