using System;
using System.Collections.Generic;
using Octokit;

namespace GithubHooks.Models
{
    public class Issue
    {
        public Uri Url { get; set; }
        public Uri LabelsUrl { get; set; }
        public Uri CommentsUrl { get; set; }
        public Uri EventsUrl { get; set; }
        public Uri HtmlUrl { get; set; }
        public int Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public User User { get; set; }
        public List<string> Labels { get; set; }
        public string State { get; set; }
        public bool Locked { get; set; }
        public string Assignee { get; set; }
        public string Milestone { get; set; }
        public int Comments { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ClosedAt { get; set; }
        public string Body { get; set; }
    }
}