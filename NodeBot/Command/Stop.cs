using ConsoleInteractive;
using EleCho.GoCqHttpSdk;
using EleCho.GoCqHttpSdk.Message;
using NodeBot.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.Command
{
    public class Stop : ICommand
    {
        public bool Execute(ICommandSender sender, string commandLine)
        {
            sender.SendMessage("机器人已停止");
            Environment.Exit(0);
            return true;
        }

        public bool Execute(IQQSender QQSender, CqMessage msgs)
        {
            QQSender.SendMessage("机器人已停止");
            ConsoleWriter.WriteLine($"机器人被{QQSender.GetNumber()}停止");
            Environment.Exit(0);
            return true;
        }

        public int GetDefaultPermission()
        {
            return 5;
        }

        public string GetName()
        {
            return "stop";
        }

        public bool IsConsoleCommand()
        {
            return true;
        }

        public bool IsGroupCommand()
        {
            return true;
        }

        public bool IsUserCommand()
        {
            return true;
        }
    }
}
