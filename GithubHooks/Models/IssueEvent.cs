using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubHooks.Models
{
    public class IssueEvent
    {
        public string action { get; set; }
        public Issue issue { get; set; }
        public Repository repository { get; set; }
        public Sender sender { get; set; }
    }
}