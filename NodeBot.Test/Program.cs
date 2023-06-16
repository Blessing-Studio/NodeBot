using NodeBot.github;

namespace NodeBot.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            WebhookService webhookService = new WebhookService();
            webhookService.Webhooks.Add(new("hi"));
            webhookService.OnStart();
            Thread.Sleep(100000000);
        }
    }
}