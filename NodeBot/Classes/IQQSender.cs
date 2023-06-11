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
        public GroupQQSender(CqWsSession session, long groupNumber, long QQNumber)
        {
            this.Session = session;
            this.QQNumber = QQNumber;
            this.GroupNumber = groupNumber;
        }
        public long? GetGroupNumber()
        {
            return GroupNumber;
        }

        public long GetNumber()
        {
            return QQNumber;
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
        public UserQQSender(CqWsSession session, long QQNumber)
        {
            this.Session = session;
            this.QQNumber = QQNumber;
        }

        public long? GetGroupNumber()
        {
            return null;
        }

        public long GetNumber()
        {
            return QQNumber;
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
