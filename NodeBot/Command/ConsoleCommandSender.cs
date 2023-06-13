using ConsoleInteractive;
using EleCho.GoCqHttpSdk;
using NodeBot.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.Command
{
    public class ConsoleCommandSender : ICommandSender
    {
        public CqWsSession Session;
        public NodeBot Bot;
        public ConsoleCommandSender(CqWsSession session, NodeBot bot)
        {
            Session = session;
            Bot = bot;
        }

        public NodeBot GetNodeBot()
        {
            return Bot;
        }

        public CqWsSession GetSession()
        {
            return Session;
        }

        public void SendMessage(string message)
        {
            ConsoleWriter.WriteLine(message);
        }
    }
}
