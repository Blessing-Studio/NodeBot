using ConsoleInteractive;
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
        public void SendMessage(string message)
        {
            ConsoleWriter.WriteLine(message);
        }
    }
}
