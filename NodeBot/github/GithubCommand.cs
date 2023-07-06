using EleCho.GoCqHttpSdk.Message;
using NodeBot.Classes;
using NodeBot.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.github
{
    public class GithubCommand : ICommand
    {
        public GithubCommand()
        {

        }
        public bool Execute(ICommandSender sender, string commandLine)
        {
            return true;
        }

        public bool Execute(IQQSender QQSender, CqMessage msgs)
        {
            throw new NotImplementedException();
        }

        public int GetDefaultPermission()
        {
            return 0;
        }

        public string GetName()
        {
            return "github";
        }

        public bool IsConsoleCommand()
        {
            return true;
        }

        public bool IsGroupCommand()
        {
            return false;
        }

        public bool IsUserCommand()
        {
            return false;
        }
    }
}
