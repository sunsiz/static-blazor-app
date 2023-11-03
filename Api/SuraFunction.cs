using System.Net;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class SuraFunction
    {
        private readonly ILogger _logger;

        public SuraFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SuraFunction>();
        }

        [Function("SuraFunction")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            // Register DBContext and the SQLite database
            //builder.Services.AddScoped<DBContext>(s => ActivatorUtilities.CreateInstance<DBContext>(s, dbPath));
            string dbPath = "quran.db";
            var db = new DBContext(new LoggerFactory(), dbPath);
            var result = db.GetSurasAsync().Result;
            //return new OkObjectResult(suras);
            response.WriteString("Welcome to Azure Functions!");
            
            response.WriteAsJsonAsync(result);
            return response;
        }
    }
}
