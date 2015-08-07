using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Octokit;

namespace GithubHooks.Models
{
    public class PullRequestEvent
    {
        public string Action { get; set; }
        public int Number { get; set; }
        public PullRequest PullRequest { get; set; }
        public Repository Repository { get; set; }
        public Sender Sender { get; set; }
    }
}