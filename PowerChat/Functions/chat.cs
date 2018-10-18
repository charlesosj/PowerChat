
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PowerChat.DB;
using PowerChat.Helpers;
using System.Net.Http;
using System.Text;
using System.Net;
using System.Net.Http.Formatting;

namespace PowerChat.Functions
{
    public static class GetMsg
    {
        [FunctionName("chat")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            Response response = new Response();


            try
            {
                //Get request body and from date 
                string requestBody = new StreamReader(req.Body).ReadToEnd();
                string quertyDate = req.Query["from"];

                //if date exists use it else set default
                DateTime dateFrom = !string.IsNullOrEmpty(quertyDate) ? DateTime.Parse(quertyDate) : DateTime.UtcNow.AddHours(-5);

                using (var db = new PowerChatDB())
                {
                    //post chat message if there is one
                    ChatLog chat = JsonConvert.DeserializeObject<ChatLog>(requestBody) ?? new ChatLog();

                    if (!string.IsNullOrEmpty(chat.Message))
                    {
                        chat.TimeStamp = DateTime.UtcNow;
                        db.ChatLogs.Add(chat);
                        db.SaveChanges();
                    }
                    //get messages from db, using date from
                    response = new Response(db.GetChatLogs(dateFrom));

                }

            }
            catch (Exception e)
            {

                response = new Response(error:true,errorMessage:e.Message);
            }
     

           //return response
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, JsonMediaTypeFormatter.DefaultMediaType.MediaType)
            };
         }
    } 
}
