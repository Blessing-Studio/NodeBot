using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.github.utils
{
    public class Commit
    {
        public string id = string.Empty;
        public string tree_id = string.Empty;
        public bool distinct = true;
        public string message = string.Empty;
        public string timestamp = string.Empty;
        public string url = string.Empty;
        public Author author = new();
        public Committer committer = new();
        public string[] added = new string[0];
        public string[] removed = new string[0];
        public string[] modified = new string[0];
        public Commit()
        {

        }
    }
}
