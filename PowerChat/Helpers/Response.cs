using PowerChat.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace PowerChat.Helpers
{
    class Response
    {
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
        public List<ChatLog> Chat = new List<ChatLog>();
        public Response(List<ChatLog> chatLog = null, bool error = false,string errorMessage = null)
        {
            Chat = chatLog;
            Error = error;
            ErrorMessage = errorMessage;
        }

    }
}
