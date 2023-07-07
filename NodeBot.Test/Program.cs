using ConsoleInteractive;
using EleCho.GoCqHttpSdk;
using EleCho.GoCqHttpSdk.Message;
using NodeBot.Command;
using NodeBot.github;

namespace NodeBot.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string ip = Console.ReadLine()!;
            Console.Clear();
            ConsoleWriter.Init();
            NodeBot nodeBot = new(ip);
            nodeBot.RegisterCommand(new Echo());
            nodeBot.RegisterCommand(new AtAll());
            nodeBot.RegisterCommand(new Op());
            nodeBot.RegisterCommand(new github.GithubCommand());
            nodeBot.RegisterCommand(new GitSubscribe());
            WebhookService webhookService = WebhookService.Instance;
            WebhookService.MessageEvent += (_, e) =>
            {
                //nodeBot.session.SendPrivateMessage(1306334428, new(new CqTextMsg(e.Message)));
            };
            nodeBot.RegisterService(webhookService);
            nodeBot.LoadPermission();
            nodeBot.Start();
            CancellationTokenSource cts = new CancellationTokenSource();

            ConsoleReader.MessageReceived += (sender, s) => {
                nodeBot.CallConsoleInputEvent(s);
            };
            ConsoleReader.BeginReadThread(cts.Token);
        }
    }
}