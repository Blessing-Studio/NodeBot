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
            throw new NotImplementedException();
        }

        public bool IsUserCommand()
        {
            throw new NotImplementedException();
        }
    }
}
