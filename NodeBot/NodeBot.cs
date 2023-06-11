using EleCho.GoCqHttpSdk;
using EleCho.GoCqHttpSdk.Message;
using EleCho.GoCqHttpSdk.Post;
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
            ReceiveMessageEvent += (sender, e) =>
            {
                if(e.Context is CqPrivateMessagePostContext cqPrivateMessage)
                {
                    ExecuteCommand(new UserQQSender(session, cqPrivateMessage.UserId), cqPrivateMessage.Message);
                }
                if(e.Context is CqGroupMessagePostContext cqGroupMessage)
                {
                    ExecuteCommand(new GroupQQSender(session, cqGroupMessage.GroupId, cqGroupMessage.UserId), cqGroupMessage.Message);
                }
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
                return;
            }
            if(sender is ConsoleCommandSender console)
            {
                if (command.IsConsoleCommand())
                {
                    command.Execute(sender, commandLine);
                }
            }
        }
        public void ExecuteCommand(IQQSender sender, CqMessage commandLine)
        {
            if (commandLine[0] is CqTextMsg cqTextMsg)
            {
                ICommand? command = GetCommandByCommandLine(cqTextMsg.Text);
                if (command == null)
                {
                    return;
                }
                if(sender is UserQQSender userQQSender && command.IsUserCommand())
                {
                    command.Execute(sender, commandLine);
                }
                if(sender is GroupQQSender groupQQSender && command.IsGroupCommand())
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
