using EleCho.GoCqHttpSdk.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.Event
{
    public class ReceiveMessageEvent : EventArgs
    {
        public CqPostContext Context;
        public ReceiveMessageEvent(CqPostContext context)
        {
            Context = context;
        }
    }
}
