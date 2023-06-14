using ConsoleInteractive;
using NodeBot.Command;

namespace NodeBot
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