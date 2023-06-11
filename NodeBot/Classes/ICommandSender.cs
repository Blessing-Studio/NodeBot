using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.Classes
{
    public interface ICommandSender
    {
        void SendMessage(string message);
    }
}
