using ConsoleInteractive;
using EleCho.GoCqHttpSdk;
using EleCho.GoCqHttpSdk.Message;
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
        public static WebhookService Instance { get; private set; } = new();
        public NodeBot? NodeBot { get; private set; }
        static WebhookService()
        {
            MessageEvent += (_, e) =>
            {
                if(e.MessageType == "push")
                {
                    PushEvent pushEvent = Newtonsoft.Json.JsonConvert.DeserializeObject<PushEvent>(e.Message)!;
                    ConsoleWriter.WriteLine(pushEvent.repository.full_name + "有新push");
                    foreach(GitSubscribeInfo info in GitSubscribe.Info)
                    {
                        if(info.Repository == pushEvent.repository.full_name && Instance.NodeBot != null)
                        {
                            string msg = $"{pushEvent.repository.full_name}有新推送!";
                            long added = 0;
                            long removed = 0;
                            long modified = 0;
                            foreach (Commit commit in pushEvent.commits)
                            {
                                msg += $"\n - {commit.id.Substring(0, 6)}  {commit.message}";
                                added += commit.added.LongLength;
                                removed += commit.removed.LongLength;
                                modified += commit.modified.LongLength;
                            }
                            msg += $"\n推送者  {pushEvent.sender.login}";
                            msg += $"\n时间  {pushEvent.head_commit.timestamp}";
                            msg += $"\n链接  {pushEvent.head_commit.url}";
                            //msg += $"\n添加  {added}";
                            //msg += $"\n移除  {removed}";
                            //msg += $"\n修改  {modified}";

                            Instance.NodeBot.SendGroupMessage(info.GroupNumber, new(new CqTextMsg(msg)));
                        }
                    }
                }
            };
        }
        public void OnStart(NodeBot nodeBot)
        {
            ListenerThread.Start();
            NodeBot = nodeBot;
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
