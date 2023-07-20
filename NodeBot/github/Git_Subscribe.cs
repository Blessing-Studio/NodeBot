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
    public class Git_Subscribe : ICommand
    {
        public static List<GitSubscribeInfo> Info = new List<GitSubscribeInfo>();
        public Git_Subscribe()
        {
            LoadInfo();
        }
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
                GitSubscribeInfo info =  new(repository, ((GroupQQSender)QQSender).GroupNumber);
                foreach (var item in Info)
                {
                    if(item == info)
                    {
                        QQSender.SendMessage($"已经订阅{repository}");
                        return false;
                    }
                }
                Info.Add(new(repository, ((GroupQQSender)QQSender).GroupNumber));
                QQSender.SendMessage($"成功订阅{repository}");
            }
            catch
            {
                QQSender.SendMessage("检查参数");
            }
            SaveInfo();
            return true;
        }

        public int GetDefaultPermission()
        {
            return 5;
        }

        public string GetName()
        {
            return "github::subscribe";
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
        public void SaveInfo()
        {
            if (!File.Exists("GithubSubScribeInfo.json"))
            {
                File.Create("GithubSubScribeInfo.json").Close();
            }
            File.WriteAllText("GithubSubScribeInfo.json", Newtonsoft.Json.JsonConvert.SerializeObject(Info));
        }
        public void LoadInfo()
        {
            if (File.Exists("GithubSubScribeInfo.json"))
            {
                string json = File.ReadAllText("GithubSubScribeInfo.json");
                Info = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GitSubscribeInfo>>(json)!;
            }
        }
    }
}
