using EleCho.GoCqHttpSdk;
using EleCho.GoCqHttpSdk.Message;
using EleCho.GoCqHttpSdk.Post;
using NodeBot.Classes;
using NodeBot.Command;
using NodeBot.Event;
using NodeBot.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot
{
    public class NodeBot
    {
        public Dictionary<long, int> Permissions = new();
        public int OpPermission = 5;
        public CqWsSession session;
        public event EventHandler<ConsoleInputEvent>? ConsoleInputEvent;
        public event EventHandler<ReceiveMessageEvent>? ReceiveMessageEvent;
        public List<ICommand> Commands = new List<ICommand>();
        public List<IService> Services = new List<IService>();
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
                ExecuteCommand(new ConsoleCommandSender(session, this), e.Text);
            };
            ReceiveMessageEvent += (sender, e) =>
            {
                if(e.Context is CqPrivateMessagePostContext cqPrivateMessage)
                {
                    ExecuteCommand(new UserQQSender(session, this, cqPrivateMessage.UserId), cqPrivateMessage.Message);
                }
                if(e.Context is CqGroupMessagePostContext cqGroupMessage)
                {
                    ExecuteCommand(new GroupQQSender(session, this, cqGroupMessage.GroupId, cqGroupMessage.UserId), cqGroupMessage.Message);
                }
            };
        }
        public void SavePermission()
        {
            if (!File.Exists("Permission.json"))
            {
                File.Create("Permission.json").Close();
            }
            File.WriteAllText("Permission.json", Newtonsoft.Json.JsonConvert.SerializeObject(Permissions));
        }
        public void LoadPermission()
        {
            if (File.Exists("Permission.json"))
            {
                string json = File.ReadAllText("Permission.json");
                Permissions = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<long, int>>(json)!;
            }
        }
        public void RegisterCommand(ICommand command)
        {
            Commands.Add(command);
        }
        public void RegisterService(IService service)
        {
            Services.Add(service);
        }
        public void Start()
        {
            session.Start();
            foreach(IService service in Services)
            {
                service.OnStart(this);
            }
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
                if (HasPermission(command, sender))
                {
                    if (sender is UserQQSender userQQSender && command.IsUserCommand())
                    {
                        command.Execute(sender, commandLine);
                    }
                    if (sender is GroupQQSender groupQQSender && command.IsGroupCommand())
                    {
                        command.Execute(sender, commandLine);
                    }
                }
                else
                {
                    sender.SendMessage("你没有权限");
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
        public bool HasPermission(ICommand command, long QQNumber)
        {
            int permission = 0;
            if (Permissions.ContainsKey(QQNumber))
            {
                permission = Permissions[QQNumber];
            }
            return permission >= command.GetDefaultPermission();
        }
        public bool HasPermission(ICommand command, ICommandSender sender)
        {
            if(sender is IQQSender QQSender)
            {
                return HasPermission(command, QQSender.GetNumber());
            }
            if(sender is ConsoleCommandSender)
            {
                return true;
            }
            return false;
        }
    }
}
