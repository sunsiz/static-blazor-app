using System;
using System.IO;
using System.Threading.Tasks;
using BlazorApp.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace ApiIsolated
{
    public class Program
    {
        //Copied from https://www.youtube.com/watch?v=xSAyEDFLFTw
        public const string DbPath = "quran.db";
        public const string AzureDbPath = "d:/home/quran.db";
        public const string DevEnvironment = "Development";
        public static void Main()
        {
            var env = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");
            bool isDevEnv = env == DevEnvironment ? true : false;
            Console.WriteLine("The environment is " + env);
            if (!isDevEnv && !File.Exists(AzureDbPath)) CopyDb();
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(s=>
                {
                    s.AddPooledDbContextFactory<DBContext>((p, o) => o
                        .UseSqlite($"data source={(isDevEnv ? DbPath : AzureDbPath)};")
                        .UseLoggerFactory(p.GetRequiredService<ILoggerFactory>()));
                })
                .Build();

            host.Run();
        }

        private static void CopyDb()
        {
            File.Copy(DbPath,AzureDbPath);
            File.SetAttributes(AzureDbPath,FileAttributes.Normal);
        }
    }
}