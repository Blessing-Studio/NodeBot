using EleCho.GoCqHttpSdk;
using NodeBot.Classes;
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
        public CqWsSession session;
        public event EventHandler<ConsoleInputEvent>? ConsoleInputEvent;
        public event EventHandler<ReceiveMessageEvent>? ReceiveMessageEvent;
        public List<ICommand> Commands = new List<ICommand>();
        public NodeBot(string ip)
        {
            session = new(new()
            {
                BaseUri = new Uri("ws://" + ip),
                UseApiEndPoint = true,
                UseEventEndPoint = true,
            });
            session.PostPipeline.Use(async (context, next) =>
            {
                if(ReceiveMessageEvent != null)
                {
                    ReceiveMessageEvent(this, new(context));
                }
                await next();
            });
            ConsoleInputEvent += (sender, e) =>
            {
                ExecuteCommand(new ConsoleCommandSender(), e.Text);
            };
        }
        public void RegisterCommand(ICommand command)
        {
            Commands.Add(command);
        }
        public void Start()
        {
            session.Start();
        }
        public void CallConsoleInputEvent(string text)
        {
            if(ConsoleInputEvent != null)
            {
                ConsoleInputEvent(this, new(text));
            }
        }
        public void ExecuteCommand(ICommandSender sender, string commandLine)
        {
            ICommand? command = GetCommandByCommandLine(commandLine);
            if(command == null)
            {
                throw new Exception();
            }
            if(sender is ConsoleCommandSender console)
            {
                if (command.IsConsoleCommand())
                {
                    command.Execute(sender, commandLine);
                }
            }
        }
        public ICommand? GetCommandByCommandLine(string command)
        {
            string[] tmp = command.Split(' ');
            foreach(string s in tmp)
            {
                if(s != string.Empty)
                {
                    return FindCommand(s);
                }
            }
            return null;
        }
        public ICommand? FindCommand(string commandName)
        {
            foreach(ICommand command in Commands)
            {
                if(command.GetName() == commandName)
                {
                    return command;
                }
            }
            return null;
        }
    }
}
