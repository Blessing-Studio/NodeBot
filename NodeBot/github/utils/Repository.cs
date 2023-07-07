using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBot.github.utils
{
    public class Repository
    {
        public long id = 0;
        public string node_id = string.Empty;
        public string name = string.Empty;
        public string full_name = string.Empty;
        public bool @private;
        public User owner = new();
        public string html_url = string.Empty;
        public string? description = string.Empty;
        public bool fork = false;
        public string url = string.Empty;
        public string forks_url = string.Empty;
        public string keys_url = string.Empty;
        public string collaborators_url = string.Empty;
        public string teams_url = string.Empty;
        public string hooks_url = string.Empty;
        public string issue_events_url = string.Empty;
        public string events_url = string.Empty;
        public string assignees_url = string.Empty;
        public string branches_url = string.Empty;
        public string tags_url = string.Empty;
        public string blobs_url = string.Empty;
        public string git_tags_url = string.Empty;
        public string git_refs_url = string.Empty;
        public string trees_url = string.Empty;
        public string statuses_url = string.Empty;
        public string languages_url = string.Empty;
        public string stargazers_url = string.Empty;
        public string contributors_url = string.Empty;
        public string subscribers_url = string.Empty;
        public string subscription_url = string.Empty;
        public string commits_url = string.Empty;
        public string git_commits_url = string.Empty;
        public string comments_url = string.Empty;
        public string issue_comment_url = string.Empty;
        public string contents_url = string.Empty;
        public string compare_url = string.Empty;
        public string merges_url = string.Empty;
        public string archive_url = string.Empty;
        public string downloads_url = string.Empty;
        public string issues_url = string.Empty;
        public string pulls_url = string.Empty;
        public string milestones_url = string.Empty;
        public string notifications_url = string.Empty;
        public string labels_url = string.Empty;
        public string releases_url = string.Empty;
        public string deployments_url = string.Empty;
        public long created_at = 0;
        public string updated_at = string.Empty;
        public long pushed_at = 0;
        public string git_url = string.Empty;
        public string ssh_url = string.Empty;
        public string clone_url = string.Empty;
        public string svn_url = string.Empty;
        public string? homepage = string.Empty;
        public long size = 0;
        public long stargazers_count = 0;
        public long watchers_count = 0;
        public string language = string.Empty;
        public bool has_issues = false;
        public bool has_projects = false;
        public bool has_downloads = false;
        public bool has_wiki = false;
        public bool has_pages = false;
        public bool has_discussions = false;
        public long forks_count = 0;
        public string? mirror_url = string.Empty;
        public bool archived = false;
        public bool disabled = false;
        public long open_issues_count = 0;
        public License license = new();
        public bool allow_forking = true;
        public bool is_template = false;
        public bool web_commit_signoff_required = false;
        public string[] topics = Array.Empty<string>();
        public string visibility = string.Empty;
        public long forks = 0;
        public long open_issues = 0;
        public long watchers = 0;
        public string default_branch = "master";
        public long stargazers = 0;
        public string master_branch = "master";
        public string? organization = string.Empty;


        public Repository()
        {

        }
    }
}
