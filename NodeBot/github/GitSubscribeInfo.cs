using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        public static bool operator ==(GitSubscribeInfo left, GitSubscribeInfo right)
        {
            if(left.GroupNumber == right.GroupNumber && left.Repository == right.Repository)
            {
                return true;    
            }
            return false;
        }
        public static bool operator !=(GitSubscribeInfo left, GitSubscribeInfo right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj != null)
            {
                if (this.GetType() == obj.GetType())
                {
                    return this == (GitSubscribeInfo)obj;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Repository.GetHashCode() ^ GroupNumber.GetHashCode();
        }
    }
}
