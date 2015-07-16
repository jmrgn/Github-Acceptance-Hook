using System;

namespace GithubHooks.Models
{
    public class Comment
    {
        public Uri Url { get; set; }
        public Uri HtmlUrl { get; set; }
        public Uri IssueUrl { get; set; }
        public int Id { get; set; }
        public GithubUser User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Body { get; set; }
    }
}