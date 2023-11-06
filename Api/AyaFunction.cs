using System;
using System.Net;
using System.Threading.Tasks;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApiIsolated
{
    public class AyaTrigger
    {
        private readonly ILogger _logger;

        public AyaTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SuraTrigger>();
        }

        [Function("Aya")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get",Route = "aya/{id:int?}")] HttpRequestData req, int? id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var sura = id;//req.Query["id"];
            var suraId=Convert.ToInt32(sura);
            //response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            // Register DBContext and the SQLite database
            //builder.Services.AddScoped<DBContext>(s => ActivatorUtilities.CreateInstance<DBContext>(s, dbPath));
            
            var db = new DBContext(new LoggerFactory());
            var result = db.GetAyaList(suraId);
            //return new OkObjectResult(suras);
            //response.WriteString("Welcome to Azure Functions!");
            
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(result);
            return response;
        }
    }
}
