using EleCho.GoCqHttpSdk;
using EleCho.GoCqHttpSdk.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.Classes
{
    public interface IQQSender : ICommandSender
    {
        long GetNumber();
        long? GetGroupNumber();
        void SendMessage(CqMessage msgs);
    }
    public class GroupQQSender : IQQSender
    {
        public long GroupNumber;
        public long QQNumber;
        public CqWsSession Session;
        public NodeBot Bot;
        public GroupQQSender(CqWsSession session,NodeBot bot, long groupNumber, long QQNumber)
        {
            this.Session = session;
            this.QQNumber = QQNumber;
            this.GroupNumber = groupNumber;
            this.Bot = bot;
        }
        public long? GetGroupNumber()
        {
            return GroupNumber;
        }

        public NodeBot GetNodeBot()
        {
            return Bot;
        }

        public long GetNumber()
        {
            return QQNumber;
        }

        public CqWsSession GetSession()
        {
            return Session;
        }

        public void SendMessage(string message)
        {
            Session.SendGroupMessage(GroupNumber, new(new CqTextMsg(message)));
        }

        public void SendMessage(CqMessage msgs)
        {
            Session.SendGroupMessage(GroupNumber, msgs);
        }
    }
    public class UserQQSender : IQQSender
    {
        public long QQNumber;
        public CqWsSession Session;
        public NodeBot Bot;
        public UserQQSender(CqWsSession session,NodeBot bot, long QQNumber)
        {
            this.Session = session;
            this.QQNumber = QQNumber;
            this.Bot = bot;
        }

        public long? GetGroupNumber()
        {
            return null;
        }

        public NodeBot GetNodeBot()
        {
            throw new NotImplementedException();
        }

        public long GetNumber()
        {
            return QQNumber;
        }

        public CqWsSession GetSession()
        {
            return Session;
        }

        public void SendMessage(string message)
        {
            Session.SendPrivateMessage(QQNumber, new CqMessage(new CqTextMsg(message)));
        }

        public void SendMessage(CqMessage msgs)
        {
            Session.SendPrivateMessage(QQNumber, msgs);
        }
    }
}
