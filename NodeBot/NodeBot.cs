using NodeBot.Command;
using NodeBot.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot
{
    public class NodeBot
    {
        private event EventHandler<ConsoleInputEvent>? ConsoleInputEvent;
        
        public List<ICommand> Commands = new List<ICommand>();
        public NodeBot() { }
        public void RegisterCommand(ICommand command)
        {
            Commands.Add(command);
        }

    }
}
