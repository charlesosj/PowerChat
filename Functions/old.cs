
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using PowerChat.DB;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Data.Common;
using PowerChat.Helpers;

namespace PowerChat
{
    public static class PostMsg
    {
        [FunctionName("postmsg")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            try
            {
                log.LogTrace("started");

                //string name = req.Query["name"];


                string requestBody = new StreamReader(req.Body).ReadToEnd();
                ChatLog chat = JsonConvert.DeserializeObject<ChatLog>(requestBody);

                Response response = new Response();


                using (var db = new PowerChatDB())
                {
                    db.ChatLogs.Add(chat);
                    db.SaveChanges();


                }

                return (ActionResult)new OkObjectResult("");
            }
            catch (System.Exception E)
            {
                log.LogTrace(E.InnerException.ToString());
                throw;
            }
                
        }
    }
}
