using ConsoleInteractive;
using NodeBot.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.github
{
    public class WebhookService : IService
    {
        public List<WebhookInfo> Webhooks { get; set; }
        public List<WebhookWorker> Workers { get; private set; } = new();
        public WebhookService()
        {
            Webhooks = new List<WebhookInfo>();
        }
        public void OnStart()
        {
            if (File.Exists("Webhooks.json"))
            {
                string jsonData = File.ReadAllText("Webhook.json");
                Webhooks = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WebhookInfo>>(jsonData)!;
            }
            foreach (WebhookInfo webhook in Webhooks)
            {
                Workers.Add(new WebhookWorker(webhook));
            }
            foreach(WebhookWorker webhook in Workers)
            {
                webhook.Start();
            }
        }

        public void OnStop()
        {
            foreach (WebhookWorker webhook in Workers)
            {
                webhook.Stop();
            }
        }
        public WebhookWorker Create(string name)
        {
            WebhookInfo info = new(name);
            Webhooks.Add(info);
            WebhookWorker worker = new WebhookWorker(info);
            worker.Start();
            Workers.Add(worker);
            return worker;
        }
    }
    public class WebhookInfo
    {
        public string name;
        public WebhookInfo(string name)
        {
            this.name = name;
        }
    }
    public class WebhookWorker
    {
        public WebhookInfo Info { get; set; }
        public Httplistener HttpListener { get; set; }
        public WebhookWorker(WebhookInfo info)
        {
            Info = info;
            HttpListener = new();
        }
        public void Start()
        {
            HttpListener.HttpEvent += HttpMsgEvent;
            HttpListener.Start($"http://+:40012/{Info.name}/");
        }
        private static void HttpMsgEvent(string msg)
        {
            
        }
        public void Stop()
        {
            HttpListener.Stop();
        }
    }
    public class Httplistener : IDisposable
    {
        public delegate void HttpDeletegate(string str);
        public event HttpDeletegate HttpEvent;
        public HttpListener SSocket;
        public string Senddata = "";
        public Httplistener()
        {
            SSocket = new HttpListener();
        }
        public bool Send(string s)
        {
            Senddata = s;
            return true;
        }
        public void Dispose()
        {
            if (SSocket != null) Stop();
        }
        public void Stop()
        {
            SSocket.Stop();
        }
        public bool IsSupported()
        {
            return HttpListener.IsSupported;
        }
        public bool Start(string url = "")
        {
            if (SSocket == null) return false;
            if (string.IsNullOrWhiteSpace(url)) return false;
            if (!IsSupported()) return false;
            //SSocket.Prefixes.Add("http://+:8080/POST/");
            SSocket?.Prefixes.Add(url);
            SSocket?.Start();
            SSocket?.BeginGetContext(Receive, null);//异步监听客户端请求
            ConsoleWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss:fff}：初始化完成，等待请求\r\n");
            return true;
        }
        /// <summary>
        /// 异步监听
        /// </summary>
        /// <param name="ar"></param>
        private void Receive(IAsyncResult ar)
        {
            //继续异步监听
            SSocket.BeginGetContext(Receive, null);
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss:fff}：新请求");
            var context = SSocket.EndGetContext(ar); //获得context对象
            var request = context.Request;
            var response = context.Response;
            ConsoleWriter.WriteLine("{0} {1} HTTP/1.1"+request.HttpMethod+request.RawUrl);
            ConsoleWriter.WriteLine("Accept: {0}"+string.Join(",", request.AcceptTypes));
            ConsoleWriter.WriteLine("User-Agent: {0}"+request.UserAgent);
            ConsoleWriter.WriteLine("Accept-Encoding: {0}"+request.Headers["Accept-Encoding"]);
            ConsoleWriter.WriteLine("Connection: {0}"+ (request.KeepAlive ? "Keep-Alive" : "close"));
            ConsoleWriter.WriteLine("Host: {0}"+ request.Url);
            ConsoleWriter.WriteLine("RemoteEndPoint: {0}"+ request.RemoteEndPoint);
            ;
            var result = "";
            var isReceive = Receivedata(request, response);
            if (!isReceive)
            {
                result = "error";
            }
            else
            {
                var sendExpire = DateTime.Now.Ticks + 30000 * 10000;
                for (; DateTime.Now.Ticks < sendExpire; Thread.Sleep(20)) if (Senddata.Length >= 0) break;
                result = Senddata;
                Senddata = "";
            }
            ConsoleWriter.WriteLine($"SEND：{result}");
            Send(200, result, response);
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleWriter.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss:fff}：处理完成\r\n");
        }
        /// <summary>
        /// //接收数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private bool Receivedata(HttpListenerRequest request, HttpListenerResponse response)
        {
            string data;
            try
            {
                var byteList = new List<byte>();
                var byteArr = new byte[2048];
                int readLen;
                var len = 0;
                do
                {
                    readLen = request.InputStream.Read(byteArr, 0, byteArr.Length);
                    len += readLen;
                    byteList.AddRange(byteArr);
                } while (readLen != 0);
                data = Encoding.UTF8.GetString(byteList.ToArray(), 0, len);
                HttpEvent?.Invoke(data); //触发事件
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                ConsoleWriter.WriteLine($"ERROR:{ex}");
                Send(404, ex.Message, response);
                return false;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            ConsoleWriter.WriteLine($"RECV：{data.Trim()}");
            return true;
        }
        private void Send(int code, string data, HttpListenerResponse response)
        {
            response.ContentType = "text/plain;charset=UTF-8";
            response.AddHeader("Content-type", "text/plain");
            response.ContentEncoding = Encoding.UTF8;
            //response.StatusDescription = code.ToString();
            response.StatusCode = code;
            var returnByteArr = Encoding.UTF8.GetBytes(data);
            try
            {
                using (var stream = response.OutputStream) { stream.Write(returnByteArr, 0, returnByteArr.Length); }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                ConsoleWriter.WriteLine($"ERROR：{ex}");
            }
        }
    }
}
