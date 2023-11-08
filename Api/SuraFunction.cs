using System.Net;
using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApiIsolated
{
    public class SuraTrigger
    {
        private readonly ILogger _logger;

        public SuraTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SuraTrigger>();
        }

        [Function("Sura")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            //response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            // Register DBContext and the SQLite database
            //builder.Services.AddScoped<DBContext>(s => ActivatorUtilities.CreateInstance<DBContext>(s, dbPath));
            //string dbPath = "quran.db";
            var db = new DBContext(new LoggerFactory(), Program.DbPath);
            var result = db.GetSuras();
            //return new OkObjectResult(suras);
            //response.WriteString("Welcome to Azure Functions!");
            
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(result);
            return response;
        }
    }
}
