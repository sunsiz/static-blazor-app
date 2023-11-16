using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

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
            _logger.LogInformation("C# HTTP trigger function <<Aya>> processed a request.");
            var sura = id;//req.Query["id"];
            var suraId=Convert.ToInt32(sura);
            //response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            // Register DBContext and the SQLite database
            //builder.Services.AddScoped<DBContext>(s => ActivatorUtilities.CreateInstance<DBContext>(s, dbPath));
            
            var db = new DBContext(new LoggerFactory(), Program.FinalDbPath);
            var result = db.GetAyaList(suraId);
            //return new OkObjectResult(suras);
            //response.WriteString("Welcome to Azure Functions!");
            
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(result);
            return response;
        }
    }
}
