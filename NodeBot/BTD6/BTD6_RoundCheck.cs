using EleCho.GoCqHttpSdk.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodeBot.BTD6.util;
using NodeBot.Classes;
using NodeBot.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.BTD6
{
    public class BTD6_RoundCheck : ICommand
    {
        public bool Execute(ICommandSender sender, string commandLine)
        {
            string msg = string.Empty;
            int round;
            try
            {
                round = int.Parse(commandLine.Split(' ')[1]);
            }
            catch
            {
                sender.SendMessage("参数错误");
            }
            finally
            {
                round = int.Parse(commandLine.Split(' ')[1]) - 1;
                if (round <= 0)
                {
                    sender.SendMessage("参数错误");
                }
                else if(round > 140)
                {
                    sender.SendMessage("最大只支持140波");
                }
                else
                {
                    msg += $"==========\n{round + 1}回合 信息如下\n";
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NodeBot.Resource.Round-Release.json")!)
                    {
                        string jsonData = new StreamReader(stream).ReadToEnd();
                        JObject json = JObject.Parse(jsonData);
                        JToken normal = json["normal"]!;
                        msg += $"经验 {normal["xp"]![round]}\n";
                        msg += $"击破气球所获得的钱 {normal["pop_money"]![round]}\n";
                        msg += $"回合结束所得到的钱 {normal["round_money"]![round]}\n";
                        msg += $"总击破 {normal["pops"]![round]}\n";
                        msg += $"钱获得倍率 {normal["money_k"]![round]}\n";
                        msg += $"所需时间 {normal["time"]![round]}\n";
                        msg += $"气球:  ";
                        JToken bloons = normal["bloons"]![round]!;
                        Dictionary<string, int> bloonsDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, int>>(bloons.ToString())!;
                        foreach(KeyValuePair<string, int> pair in bloonsDictionary)
                        {
                            msg += $"{BloonsUtils.Translate(pair.Key)} {pair.Value}个  ";
                        }
                    }
                    msg += $"\n\n==========";
                    sender.SendMessage(msg);
                }
            }
            return true;
        }

        public bool Execute(IQQSender QQSender, CqMessage msgs)
        {
            if (msgs[0] is CqTextMsg msg)
            {
                string commandLine = msg.Text;
                Execute(QQSender, commandLine);
            }
            else
            {
                QQSender.SendMessage("参数错误");
            }
            return true;
        }

        public int GetDefaultPermission()
        {
            return 0;
        }

        public string GetName()
        {
            return "btd6::RoundCheck";
        }

        public bool IsConsoleCommand()
        {
            return true;
        }

        public bool IsGroupCommand()
        {
            return true;
        }

        public bool IsUserCommand()
        {
            return true;
        }
    }
}
