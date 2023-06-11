using NodeBot.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.Command
{
    public interface ICommand
    {
        string GetName();
        bool IsConsoleCommand();
        bool IsUserCommand();
        bool IsGroupCommand();
        int GetDefaultPermission();
        bool Execute(ICommandSender sender, string commandLine);
    }
}
