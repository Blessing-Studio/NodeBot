using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.github.utils
{
    public class PushEvent : IGithubEvent
    {
        public string @ref = string.Empty;
        public string before = string.Empty;
        public string after = string.Empty;
        public Repository repository = new();
        public Author pusher = new();
        public User? organization = new();
        public User sender = new();
        public bool created = false;
        public bool deleted = false;
        public bool forced = false;
        public string? base_ref = string.Empty;
        public string compare = string.Empty;
        public Commit[] commits = Array.Empty<Commit>();
        public Commit head_commit = new();
    }
}
