using ConsoleInteractive;
using NodeBot.github.utils;
using NodeBot.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.github
{
    public class WebhookMessageEvent : EventArgs
    {
        public string Message { get; set; }
        public string MessageType { get; set; }
        public WebhookMessageEvent(string message, string messageType)
        {
            Message = message;
            MessageType = messageType;
        }
    }
    public class WebhookService : IService
    {
        public static Thread ListenerThread = new(new ParameterizedThreadStart(Listening));
        public static event EventHandler<WebhookMessageEvent>? MessageEvent;
        static WebhookService()
        {
            MessageEvent += (_, e) =>
            {
                if(e.MessageType == "PushEvent")
                {
                    PushEvent pushEvent = Newtonsoft.Json.JsonConvert.DeserializeObject<PushEvent>(e.Message)!;
                    ConsoleWriter.WriteLine(pushEvent.repository.full_name + "有新push");
                }
            };
        }
        public void OnStart()
        {
            ListenerThread.Start();
        }

        public void OnStop()
        {
#pragma warning disable SYSLIB0006 // 类型或成员已过时
            ListenerThread.Abort();
#pragma warning restore SYSLIB0006 // 类型或成员已过时
        }
        public static void Listening(object? args)
        {
            HttpListener httpListener = new();
            httpListener.Prefixes.Add("http://+:40001/");
            httpListener.Start();
            while (true)
            {
                HttpListenerContext context = httpListener.GetContext();
                string json = "";
                using(StreamReader reader = new(context.Request.InputStream))
                {
                    json = reader.ReadToEnd();
                }
                string type = context.Request.Headers["X-GitHub-Event"]!;
                context.Response.Close();
                if(MessageEvent != null)
                {
                    MessageEvent.Invoke(ListenerThread, new(json, type));
                }
            }
        }
    }
}
