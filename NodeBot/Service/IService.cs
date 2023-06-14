using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.Service
{
    public interface IService
    {
        void OnStart();
        void OnStop();
    }
}
