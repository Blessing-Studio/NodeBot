using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.github
{
    public class GitSubscribeInfo
    {
        public string Repository = string.Empty;
        public long GroupNumber = 0;
        public GitSubscribeInfo() { }
        public GitSubscribeInfo(string repository, long groupNumber)
        {
            Repository = repository;
            GroupNumber = groupNumber;
        }
    }
}
