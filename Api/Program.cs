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
        private const string DbPath = "quran.db";
        private const string AzureDbPath = "d:/home/quran.db";
        private const string DevEnvironment = "Development";
        public static void Main()
        {
            bool isDevEnv = System.Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == DevEnvironment
                ? true
                : false;
            if (!isDevEnv && !File.Exists(AzureDbPath)) CopyDb();
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(s=>
                {
                    s.AddPooledDbContextFactory<DBContext>((s, o) => o
                        .UseSqlite($"datasource={(isDevEnv ? DbPath : AzureDbPath)};")
                        .UseLoggerFactory(s.GetRequiredService<ILoggerFactory>()));
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