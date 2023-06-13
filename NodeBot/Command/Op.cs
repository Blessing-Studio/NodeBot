using EleCho.GoCqHttpSdk.Message;
using NodeBot.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.Command
{
    public class Op : ICommand
    {
        public bool Execute(ICommandSender sender, string commandLine)
        {
            string[] strNums = commandLine.Split(" ");
            foreach(string strNum in strNums)
            {
                try
                {
                    long num = long.Parse(strNum);
                    sender.GetNodeBot().Permissions[num] = sender.GetNodeBot().OpPermission;
                    sender.SendMessage($"将{num}设为op");
                }
                catch { }
            }
            return true;
        }

        public bool Execute(IQQSender QQSender, CqMessage msgs)
        {
            string commandLine = ((CqTextMsg)msgs[0]).Text;
            string[] strNums = commandLine.Split(" ");
            foreach (string strNum in strNums)
            {
                try
                {
                    long num = long.Parse(strNum);
                    QQSender.GetNodeBot().Permissions[num] = QQSender.GetNodeBot().OpPermission;
                    QQSender.SendMessage($"将{num}设为op");
                }
                catch { }
            }
            return true;
        }

        public int GetDefaultPermission()
        {
            return 5;
        }

        public string GetName()
        {
            return "op";
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
