using EleCho.GoCqHttpSdk.Message;
using NodeBot.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.Command
{
    public class Echo : ICommand
    {
        public bool Execute(ICommandSender sender, string commandLine)
        {
            sender.SendMessage(commandLine.TrimStart().Substring(5));
            return true;
        }

        public bool Execute(IQQSender QQSender, CqMessage msgs)
        {
            if (msgs[0] is CqTextMsg msg)
            {
                string tmp = msg.Text;
                tmp.TrimStart();
                tmp = tmp.Substring(5);
                msgs[0] = new CqTextMsg(tmp);
            }
            QQSender.SendMessage(msgs);
            return true;
        }

        public int GetDefaultPermission()
        {
            return 0;
        }

        public string GetName()
        {
            return "echo";
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
