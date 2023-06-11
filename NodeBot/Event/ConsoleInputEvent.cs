using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.Event
{
    public class ConsoleInputEvent : EventArgs
    {
        public string Text;
        public ConsoleInputEvent(string text)
        {
            Text = text;
        }
    }
}
