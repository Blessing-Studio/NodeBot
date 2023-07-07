using EleCho.GoCqHttpSdk.Message;
using NodeBot.Classes;
using NodeBot.Command;
using NodeBot.github.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.github
{
    public class GitSubscribe : ICommand
    {
        public static List<GitSubscribeInfo> Info = new List<GitSubscribeInfo>();
        public bool Execute(ICommandSender sender, string commandLine)
        {
            return true;
        }

        public bool Execute(IQQSender QQSender, CqMessage msgs)
        {
            try
            {
                string msg = ((CqTextMsg)msgs[0]).Text;
                string repository = msg.Split(' ')[1];
                Info.Add(new(repository, ((GroupQQSender)QQSender).GroupNumber));
                QQSender.SendMessage($"成功订阅{repository}");
            }
            catch (Exception ex)
            {
                QQSender.SendMessage("检查参数");
            }
            return true;
        }

        public int GetDefaultPermission()
        {
            return 5;
        }

        public string GetName()
        {
            return "GitSubscribe";
        }

        public bool IsConsoleCommand()
        {
            return false;
        }

        public bool IsGroupCommand()
        {
            return true;
        }

        public bool IsUserCommand()
        {
            return false;
        }
    }
}
